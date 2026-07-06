using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

static class OutputUploader
{
    private static readonly HttpClient client = new();

    public static async Task UploadFileAsync()
    {
        OutputPath.Ensure();   

        using var form = new MultipartFormDataContent();
        using var fileStream = File.OpenRead(OutputPath.OutputFile);

        form.Add(new StreamContent(fileStream), "file", "output.txt");

        await client.PostAsync(Config.UploadUrl, form);
    }
}
