using Microsoft.AspNetCore.Identity;
using NetCoreIdentity_1.Models.Enums;
using NetCoreIdentity_1.Models.Interfaces;

namespace NetCoreIdentity_1.Models.Entities
{
    public class AppUserRole:IdentityUserRole<int>, IEntity
    {
        public AppUserRole()
        {
            Status = DataStatus.Inserted;
            CreatedDate = DateTime.Now;
            
        }
        public int ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DataStatus Status { get; set; }

        // Relational Properties

        public virtual AppUser User { get; set; }

        public virtual AppRole Role { get; set; }
    }
}
