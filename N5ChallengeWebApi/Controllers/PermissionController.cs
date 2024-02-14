using Microsoft.AspNetCore.Mvc;
using N5ChallengeWebApi.Models;
using Serilog;
using N5ChallengeWebApi.Repository;
using Confluent.Kafka;
using System.Diagnostics;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace N5ChallengeWebApi.Controllers
{
    [Route("api/v1/n5permissions")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionRepository _permissionService;
        private readonly IConfiguration _configuration;

        public PermissionController(IPermissionRepository permissionService, IConfiguration configuration)
        {
            _permissionService = permissionService;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("getpermissions")]
        public async Task<IActionResult> GetPermissions()
        {
            try
            {
                Log.Logger = new LoggerConfiguration().WriteTo.File(".\\Logs\\LogGetPermission.txt").MinimumLevel.Verbose().CreateLogger();

                Log.Debug("List of requested permissions");
                Log.CloseAndFlush();
                var permissions = await _permissionService.GetPermissions();
                if (permissions == null)
                {
                    return NotFound();
                }
                await SendMessageRequest("Get");
                return Ok(permissions);
            }
            catch (Exception)
            {
                Log.Warning("Exception error");
                Log.CloseAndFlush();
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("getpermissionbyid")]
        public async Task<IActionResult> GetPermissionById(int id)
        {
            try
            {
                if (id == null)
                {
                    Log.Warning("Null value for Id");
                    return BadRequest();
                }

                Log.Logger = new LoggerConfiguration().WriteTo.File(".\\Logs\\LogRequestPermission.txt").MinimumLevel.Verbose().CreateLogger();

                Log.Debug("Requested permission with id {id}", id.ToString());

                var permission = await _permissionService.GetPermissionById(id);

                if (permission == null)
                {
                    Log.Warning("Permission not found");
                    Log.CloseAndFlush();
                    return NotFound();
                }

                Log.Information("Permission with id {id} found", id.ToString());
                Log.CloseAndFlush();
                await SendMessageRequest("Get");
                return Ok(permission);
            }
            catch (Exception)
            {
                Log.Warning("Exception error");
                Log.CloseAndFlush();
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("requestpermission")]
        public async Task<IActionResult> RequestPermission([FromBody] Permission permission)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Log.Logger = new LoggerConfiguration().WriteTo.File(".\\Logs\\LogRequestPermission.txt").MinimumLevel.Verbose().CreateLogger();
                    Log.Debug("Add new permission for {fomename} {surename}", permission.EmployeeForename, permission.EmployeeSurname);

                    var permissionId = await _permissionService.RequestPermission(permission);
                    if (permissionId > 0)
                    {
                        Log.Information("Permission added correctly");
                        Log.CloseAndFlush();
                        await SendMessageRequest("Request");
                        return Ok(permissionId);
                    }
                    else
                    {
                        Log.Warning("Request failed");
                        Log.CloseAndFlush();
                        return NotFound();
                    }

                }
                catch (Exception)
                {
                    Log.Warning("Exception error");
                    Log.CloseAndFlush();
                    return BadRequest();
                }
            }
            Log.Warning("Required fields are missing");
            Log.CloseAndFlush();
            return BadRequest();
        }

        [HttpPut]
        [Route("modifypermission")]
        public async Task<IActionResult> ModifyPermission([FromBody] Permission permissionToUpdate)
        {
            try
            {
                Log.Logger = new LoggerConfiguration().WriteTo.File(".\\Logs\\LogModifyPermission.txt").MinimumLevel.Verbose().CreateLogger();
                Log.Debug("Modify permission");

                await _permissionService.ModifyPermission(permissionToUpdate);
                Log.Information("Permission modificate correctly");
                Log.CloseAndFlush();
                await SendMessageRequest("Modifiy");
                return Ok();

            }
            catch (Exception ex)
            {
                Log.Warning("Exception error {error}", ex.Message);
                Log.CloseAndFlush();
                if (ex.GetType().FullName == "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                {
                    return NotFound();
                }

                return BadRequest();
            }

        }

        private async Task<bool> SendMessageRequest(string serviceName)
        {
            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = _configuration["ApacheKafkaUrl"],
                ClientId = Dns.GetHostName()
            };

            try
            {
                using (var producer = new ProducerBuilder<Null, string>(config).Build())
                {
                    string id = Guid.NewGuid().ToString();
                    var result = await producer.ProduceAsync(id, new Message<Null, string>{Value = serviceName });

                    Debug.WriteLine($"Delivery Timestamp: { result.Timestamp.UtcDateTime}");
                    return await Task.FromResult(true);
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(false);
            }
        }
    }
}
