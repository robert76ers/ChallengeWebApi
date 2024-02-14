using Elasticsearch.Net;
using N5ChallengeWebApi.Models;
using Nest;

namespace N5ChallengeWebApi.Repository
{
    public interface IPermissionElasticSearchRepository
    {
        List<Permission> GetPermissions();
        Permission RequestPermission(Permission product);
        Permission GetPermissionById(int id);
        void ModifyPermission(Permission product);
    }

    public class PermissionElasticSearchRepository : IPermissionElasticSearchRepository
    {
        public readonly IElasticClient _elasticClient;

        public PermissionElasticSearchRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public List<Permission> GetPermissions()
        {
            var response = _elasticClient.Search<Permission>().Documents;

            return response.ToList();
        }
        public Permission RequestPermission(Permission permission)
        {
            var response = _elasticClient.IndexDocument(permission);

            if (response.IsValid)
            {
                return permission;
            }
            else
            {
                return new Permission();
            }
        }
        public Permission GetPermissionById(int id)
        {
            var response = _elasticClient.Search<Permission>(x => x.Query(q1 => q1.Bool(b => b.Must(m => m.Terms(t => t.Field(f => f.Id).Terms<int>(id))))));

            return response.Documents.FirstOrDefault();
        }
        public void ModifyPermission(Permission permission)
        {
            if (permission != null)
            {
                var updateResponse = _elasticClient.UpdateByQueryAsync<Permission>(q => q.Query(q1 => q1.Bool(b => b.Must(m => m.Match(x => x.Field(f => f.Id == permission.Id))))));
            }
        }
    }
}
