using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using N5ChallengeWebApi.Models;

namespace N5ChallengeWebApi.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly N5UsersPermissionsContext db;
        private readonly IPermissionElasticSearchRepository _es;
        public PermissionRepository(N5UsersPermissionsContext _db, IPermissionElasticSearchRepository es)
        {
            db = _db;
            _es = es;
        }

        public async Task<List<Permission>> GetPermissions()
        {
            if (db != null)
            {
                var response = _es.GetPermissions();
                
                return await (from p in db.Permissions
                              select new Permission
                              {
                                  Id = p.Id,
                                  PermissionType = p.PermissionType,
                                  EmployeeForename = p.EmployeeForename,
                                  EmployeeSurname = p.EmployeeSurname,
                                  PermissionDate = p.PermissionDate
                              }).ToListAsync();
            }

            return null;
        }
        public async Task<Permission> GetPermissionById(int id)
        {
            if (db != null)
            {
                var response = _es.GetPermissionById(id);
                    
                return await (from p in db.Permissions
                                where p.Id == id
                                select new Permission
                                {
                                    Id = p.Id,
                                    PermissionType = p.PermissionType,
                                    EmployeeForename = p.EmployeeForename,
                                    EmployeeSurname = p.EmployeeSurname,
                                    PermissionDate = p.PermissionDate
                                }).FirstOrDefaultAsync();
            }
            return null;
        }
        public async Task<int> RequestPermission(Permission permission)
        {
            if (db != null)
            {
                await db.Permissions.AddAsync(permission);
                await db.SaveChangesAsync();

                var response = _es.RequestPermission(permission);

                return permission.Id;
            }

            return 0;
        }
        public async Task ModifyPermission(Permission permissionToUpdate)
        {
            if (db != null)
            {
                //Delete that post
                db.Permissions.Update(permissionToUpdate);

                //Commit the transaction
                await db.SaveChangesAsync();

                _es.ModifyPermission(permissionToUpdate);
            }
        }
    }
}