/*
 * CKFinder
 * ========
 * http://cksource.com/ckfinder
 * Copyright (C) 2007-2014, CKSource - Frederico Knabben. All rights reserved.
 *
 * The software, this file and its contents are subject to the CKFinder
 * License. Please read the license.txt file before using, installing, copying,
 * modifying or distribute this file or part of its contents. The contents of
 * this file is part of the Source Code of CKFinder.
 */

using System;
using System.Web;
using System.Text.RegularExpressions;

namespace CKFinder.Connector.CommandHandlers
{
	internal class FileUploadCommandHandler : CommandHandlerBase
	{
		public FileUploadCommandHandler()
			: base()
		{
		}

		public override void SendResponse( System.Web.HttpResponse response )
		{
			int iErrorNumber = 0;
			string sFileName = "";
			string sFilePath = "";
			string sUnsafeFileName = "";

			try
			{
				this.CheckConnector();
				this.CheckRequest();

				if ( !this.CurrentFolder.CheckAcl( AccessControlRules.FileUpload ) )
				{
					ConnectorException.Throw( Errors.Unauthorized );
				}

				HttpPostedFile oFile = null;
				if ( HttpContext.Current.Request.Files["upload"] != null )
				{
					oFile = HttpContext.Current.Request.Files["upload"];
				}
				else if ( HttpContext.Current.Request.Files["NewFile"] != null )
				{
					oFile = HttpContext.Current.Request.Files["NewFile"];
				}
				else if ( HttpContext.Current.Request.Files.AllKeys.Length > 0 )
				{
					oFile = HttpContext.Current.Request.Files[HttpContext.Current.Request.Files.AllKeys[0]];
				}

				if ( oFile != null )
				{					
					int iPathIndex = oFile.FileName.LastIndexOf( "\\" );
					sFileName = ( iPathIndex >= 0  && oFile.FileName.Length > 1 ) ? oFile.FileName.Substring( iPathIndex + 1 ) : oFile.FileName;

					if ( Config.Current.CheckDoubleExtension )
						sFileName = this.CurrentFolder.ResourceTypeInfo.ReplaceInvalidDoubleExtensions( sFileName );

					sUnsafeFileName = sFileName;

					if ( Config.Current.DisallowUnsafeCharacters )
						sFileName = sFileName.Replace(";","_");

					// Replace dots in the name with underscores (only one dot can be there... security issue).
					if ( Config.Current.ForceSingleExtension )
						sFileName = Regex.Replace( sFileName, @"\.(?![^.]*$)", "_", RegexOptions.None );

					if ( sFileName != sUnsafeFileName )
						iErrorNumber = Errors.UploadedInvalidNameRenamed;

					if ( Connector.CheckFileName( sFileName ) && !Config.Current.CheckIsHiddenFile( sFileName ) )
					{
						if ( !Config.Current.CheckSizeAfterScaling && this.CurrentFolder.ResourceTypeInfo.MaxSize > 0 && oFile.ContentLength > this.CurrentFolder.ResourceTypeInfo.MaxSize )
							ConnectorException.Throw( Errors.UploadedTooBig );

						string sExtension = System.IO.Path.GetExtension( sFileName );
						sExtension = sExtension.TrimStart( '.' );

						if ( !this.CurrentFolder.ResourceTypeInfo.CheckExtension( sExtension ) )
							ConnectorException.Throw( Errors.InvalidExtension );

						if ( Config.Current.CheckIsNonHtmlExtension( sExtension ) && !this.CheckNonHtmlFile( oFile ) )
							ConnectorException.Throw( Errors.UploadedWrongHtmlFile );

						// Map the virtual path to the local server path.
						string sServerDir = this.CurrentFolder.ServerPath;

						string sFileNameNoExt = CKFinder.Connector.Util.GetFileNameWithoutExtension( sFileName );
						string sFullExtension = CKFinder.Connector.Util.GetExtension( sFileName );
						int iCounter = 0;

						// System.IO.File.Exists in C# does not return true for protcted files
						if ( Regex.IsMatch( sFileNameNoExt, @"^(AUX|COM\d|CLOCK\$|CON|NUL|PRN|LPT\d)$", RegexOptions.IgnoreCase ) )
						{
							iCounter++;
							sFileName = sFileNameNoExt + "(" + iCounter + ")" + sFullExtension;
							iErrorNumber = Errors.UploadedFileRenamed;
						}

						while ( true )
						{
							sFilePath = System.IO.Path.Combine( sServerDir, sFileName );

							if ( System.IO.File.Exists( sFilePath ) )
							{
								iCounter++;
								sFileName = sFileNameNoExt + "(" + iCounter + ")" + sFullExtension;
								iErrorNumber = Errors.UploadedFileRenamed;
							}
							else
							{
								oFile.SaveAs( sFilePath );

								if ( Config.Current.SecureImageUploads && ImageTools.IsImageExtension( sExtension ) && !ImageTools.ValidateImage( sFilePath ) )
								{
									System.IO.File.Delete( sFilePath );
									ConnectorException.Throw( Errors.UploadedCorrupt );
								}

								Settings.Images imagesSettings = Config.Current.Images;

								if ( imagesSettings.MaxHeight > 0 && imagesSettings.MaxWidth > 0 )
								{
									ImageTools.ResizeImage( sFilePath, sFilePath, imagesSettings.MaxWidth, imagesSettings.MaxHeight, true, imagesSettings.Quality );

									if ( Config.Current.CheckSizeAfterScaling && this.CurrentFolder.ResourceTypeInfo.MaxSize > 0 )
									{
										long fileSize = new System.IO.FileInfo( sFilePath ).Length;
										if ( fileSize > this.CurrentFolder.ResourceTypeInfo.MaxSize )
										{
											System.IO.File.Delete( sFilePath );
											ConnectorException.Throw( Errors.UploadedTooBig );
										}
									}
								}

								break;
							}
						}
					}
					else
						ConnectorException.Throw( Errors.InvalidName );
				}
				else
					ConnectorException.Throw( Errors.UploadedCorrupt );
			}
			catch ( ConnectorException connectorException )
			{
				iErrorNumber = connectorException.Number;
			}
			catch ( System.Security.SecurityException )
			{
#if DEBUG
				throw;
#else
				iErrorNumber = Errors.AccessDenied;
#endif
			}
			catch ( System.UnauthorizedAccessException )
			{
#if DEBUG
				throw;
#else
				iErrorNumber = Errors.AccessDenied;
#endif
			}
			catch
			{
#if DEBUG
				throw;
#else
				iErrorNumber = Errors.Unknown;
#endif
			}

#if DEBUG
			if ( iErrorNumber == Errors.None || iErrorNumber == Errors.UploadedFileRenamed || iErrorNumber == Errors.UploadedInvalidNameRenamed )
				response.Clear();
#else
			response.Clear();
#endif
			System.Web.HttpRequest _Request = System.Web.HttpContext.Current.Request;
			if ( _Request.QueryString["response_type"] != null && "txt" == _Request.QueryString["response_type"].ToString() )
			{
				string _errorMsg = "";
				if ( iErrorNumber > 0 )
				{
					_errorMsg = Lang.getErrorMessage( iErrorNumber ).Replace( "%1", sFileName );
					if ( iErrorNumber != Errors.UploadedFileRenamed && iErrorNumber != Errors.UploadedInvalidNameRenamed )
						sFileName = "";
				}
				response.Write( sFileName + "|" + _errorMsg );
			}
			else
			{
				response.Write("<script type=\"text/javascript\">");
				response.Write( this.GetJavaScriptCode( iErrorNumber, sFileName, this.CurrentFolder.Url ) );
				response.Write( "</script>" );
			}

			Connector.CKFinderEvent.ActivateEvent( CKFinderEvent.Hooks.AfterFileUpload, this.CurrentFolder, sFilePath );

			response.End();
		}

		protected virtual string GetJavaScriptCode( int errorNumber, string fileName, string folderUrl )
		{
			System.Web.HttpRequest _Request = System.Web.HttpContext.Current.Request;
			string _funcNum = _Request.QueryString["CKFinderFuncNum"];
			string _errorMsg = "";
			string fileUrl = folderUrl + fileName;

			if ( errorNumber > 0 )
			{
				_errorMsg = Lang.getErrorMessage( errorNumber ).Replace( "%1", fileName );
				if ( errorNumber != Errors.UploadedFileRenamed && errorNumber != Errors.UploadedInvalidNameRenamed )
					fileName = "";
			}
			if ( _funcNum != null )
			{
				_funcNum = Regex.Replace( _funcNum, @"[^0-9]", "", RegexOptions.None );
				if ( errorNumber > 0 )
				{
					_errorMsg = Lang.getErrorMessage( errorNumber ).Replace( "%1", fileName );
					if ( errorNumber != Errors.UploadedFileRenamed )
						fileUrl = "";
				}
				return "window.parent.CKFinder.tools.callFunction(" + _funcNum + ",'" + fileUrl.Replace( "'", "\\'" ) + "','" + _errorMsg.Replace( "'", "\\'" ) + "') ;";
			}
			else
			{
				return "window.parent.OnUploadCompleted('" + fileName.Replace( "'", "\\'" ) + "','" + _errorMsg.Replace( "'", "\\'" ) + "') ;";
			}
		}

		private bool CheckNonHtmlFile( HttpPostedFile file )
		{
			byte[] buffer = new byte[ 1024 ];
			file.InputStream.Read( buffer, 0, 1024 );

			string firstKB = System.Text.ASCIIEncoding.ASCII.GetString( buffer );

			if ( Regex.IsMatch( firstKB, @"<!DOCTYPE\W*X?HTML", RegexOptions.IgnoreCase | RegexOptions.Singleline ) )
				return false;

			if ( Regex.IsMatch( firstKB, @"<(?:body|head|html|img|pre|script|table|title)", RegexOptions.IgnoreCase | RegexOptions.Singleline ) )
				return false;

			//type = javascript
			if ( Regex.IsMatch( firstKB, @"type\s*=\s*[\'""]?\s*(?:\w*/)?(?:ecma|java)", RegexOptions.IgnoreCase | RegexOptions.Singleline ) )
				return false;

			//href = javascript
			//src = javascript
			//data = javascript
			if ( Regex.IsMatch( firstKB, @"(?:href|src|data)\s*=\s*[\'""]?\s*(?:ecma|java)script:", RegexOptions.IgnoreCase | RegexOptions.Singleline ) )
				return false;

			//url(javascript
			if ( Regex.IsMatch( firstKB, @"url\s*\(\s*[\'""]?\s*(?:ecma|java)script:", RegexOptions.IgnoreCase | RegexOptions.Singleline ) )
				return false;

			return true;
		}
	}
}
