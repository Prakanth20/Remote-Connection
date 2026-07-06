using System.Net.Http;
using System.Threading.Tasks;

static class CommandFetcher
{
    private static readonly HttpClient client = new();

    public static async Task<string> FetchAsync()
    {
        return await client.GetStringAsync(Config.CommandUrl);
    }
}
