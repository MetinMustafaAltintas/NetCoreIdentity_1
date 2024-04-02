using NetCoreIdentity_1.Models.Enums;
using NetCoreIdentity_1.Models.Interfaces;

namespace NetCoreIdentity_1.Models.Entities
{
    public abstract class BaseEntity : IEntity
    {
        public BaseEntity()
        {
            Status = DataStatus.Inserted;
            CreatedDate = DateTime.Now;
        }
        public int ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DataStatus Status { get; set; }
    }
}
