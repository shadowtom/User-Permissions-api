using Nest;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Permissions.Domain.Entities;
using User.Permissions.Domain.Interfaces.Elasticsearch;

namespace User.Permissions.Infrastructure.Elasticsearch
{
    public class ElasticSearchRepository : IElasticSearchRepository
    {

        private readonly IElasticClient _client;

        public ElasticSearchRepository(IElasticClient client)
        {
            _client = client;
        }

        public async Task IndexPermissionAsync(Permission permission)
        {
            try
            {
                await _client.IndexDocumentAsync(permission);
            }
            catch (Exception ex)
            {

                Log.Error("{UtcNow} - Error indexing permission: {Message}", DateTime.UtcNow, ex.Message);
            }
        }
    }
}
