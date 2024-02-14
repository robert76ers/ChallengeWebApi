using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using N5ChallengeWebApi.Models;
using N5ChallengeWebApi.Repository;

namespace N5ChallengeTestProject
{
    public class PermissionUnitTest
    {
        private PermissionRepository _permissionService;
        
        public static DbContextOptions<N5UsersPermissionsContext> dbContextOptions { get; }
        public IPermissionElasticSearchRepository permissionElasticSearchRepository { get; }

        static PermissionUnitTest()
        {
            dbContextOptions = new DbContextOptionsBuilder<N5UsersPermissionsContext>()
                .Options;
        }

        public PermissionUnitTest()
        {
            var context = new N5UsersPermissionsContext(dbContextOptions);
            _permissionService = new PermissionRepository(context, permissionElasticSearchRepository);
        }

        [Fact]
        public async void Task_GetPermissions_Success()
        {
            //Arrange

            //Act
            var result = await _permissionService.GetPermissions();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Permission>>(result);
            Assert.NotEmpty(result);
        }

        
        [Fact]
        public async void Task_GetPermissionById_Success()
        {
            //Arrange  
            int validId = 1;
            int invalidId = 20;

            //Act  
            var errorResult = _permissionService.GetPermissionById(invalidId);
            var successResult = _permissionService.GetPermissionById(validId);

            //Assert  
            Assert.IsType<OkObjectResult>(successResult);
            Assert.IsType<NotFoundResult>(errorResult);
            Assert.Equal(1, successResult.Id);
        }

        [Fact]
        public async void Task_RequestPermission_Success()
        {
            //Arrange  
            Permission permission = new Permission
            {
                EmployeeForename = "Test",
                EmployeeSurname = "Test",
                PermissionDate = DateTime.Now,
                PermissionType = 1
            };

            //Act  
            var response = await _permissionService.RequestPermission(permission);

            //Assert  
            Assert.IsType<CreatedAtActionResult>(response);
            Assert.IsType<Permission>(response);
        }
    }
}