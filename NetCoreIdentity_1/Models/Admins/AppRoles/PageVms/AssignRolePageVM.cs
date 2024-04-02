using NetCoreIdentity_1.Models.Admins.AppRoles.ResponseModels;

namespace NetCoreIdentity_1.Models.Admins.AppRoles.PageVms
{
    public class AssignRolePageVM
    {
        public List<AppRoleResponseModel> Roles { get; set; }
        public int UserID { get; set; }
    }
}
