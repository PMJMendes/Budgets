﻿using Flurl.Http;
using Keycloak.Net.Models.Key;
using System.Threading.Tasks;

namespace Keycloak.Net
{
    public partial class KeycloakClient
    {
        public async Task<KeysMetadata> GetKeysAsync(string realm) => await GetBaseUrl(realm)
            .AppendPathSegment($"/admin/realms/{realm}/keys")
            .GetJsonAsync<KeysMetadata>()
            .ConfigureAwait(false);
    }
}
