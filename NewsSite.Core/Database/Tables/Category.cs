namespace NewsSite.Core.Database.Tables
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Order { get; set; }
    }
}
