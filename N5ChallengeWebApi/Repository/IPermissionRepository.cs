using N5ChallengeWebApi.Models;

namespace N5ChallengeWebApi.Repository
{
    public interface IPermissionRepository
    {
        Task<List<Permission>> GetPermissions();
        Task<Permission> GetPermissionById(int id);
        Task<int> RequestPermission(Permission permission);
        Task ModifyPermission(Permission permissionToUpdate);
    }
}
