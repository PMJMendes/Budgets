using Flurl.Http;
using Keycloak.Net.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Keycloak.Net
{
    public partial class KeycloakClient
    {
        public async Task<IEnumerable<User>> GetUsersByIdsAsync(string realm, IEnumerable<string> list)
        {
            var queryParams = new Dictionary<string, object>
            {
                [nameof(list)] = list
            };

            return await GetBaseUrl(realm)
                .AppendPathSegment($"/realms/{realm}/pm-extensions/admin/users/search-by-id-list")
                .SetQueryParams(queryParams)
                .GetJsonAsync<IEnumerable<User>>()
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Credentials>> GetUserCredentialsAsync(string realm, string userid)
        {
            return await GetBaseUrl(realm)
                .AppendPathSegment($"/admin/realms/{realm}/users/{userid}/credentials")
                .GetJsonAsync<IEnumerable<Credentials>>()
                .ConfigureAwait(false);
        }

        public async Task<bool> DeleteUserCredentialsAsync(string realm, string userId, string credId)
        {
            var response = await GetBaseUrl(realm)
                .AppendPathSegment($"/admin/realms/{realm}/users/{userId}/credentials/{credId}")
                .DeleteAsync()
                .ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }
    }
}
