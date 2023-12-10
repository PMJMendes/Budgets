namespace Krypton.Budgets.Tests.API._Impl;

internal static class ResponseParserStatic
{
    public static async Task<string?> ParseResponse(this Task<HttpResponseMessage> message)
    {
        return await (await message).Content.ReadAsStringAsync();
    }
}
