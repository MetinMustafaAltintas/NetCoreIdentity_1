using Microsoft.AspNetCore.Identity;
using NetCoreIdentity_1.Models.Enums;
using NetCoreIdentity_1.Models.Interfaces;

namespace NetCoreIdentity_1.Models.Entities
{
    public class AppUser : IdentityUser<int>, IEntity
    {
        public AppUser()
        {
            Status = DataStatus.Inserted;
            CreatedDate = DateTime.Now;
        }
        public int ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DataStatus Status { get; set; }

        //Relational Properties
        public virtual AppUserProfile Profile { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<AppUserRole> UserRoles { get; set; }

    }
}