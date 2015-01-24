using System;

namespace NewsSite.Core.Database
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int InsertUserId { get; set; }
        public int? UpdateUserId { get; set; }
        public bool IsActive { get; set; }
    }
}
