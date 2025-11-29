using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Microsoft.Extensions.Configuration;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("iniciando Azure AI DocumentAnalysisClient");

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

string key = config["AzureKey"]  ?? throw new InvalidOperationException("AzureKey no configurado en User Secrets");
string endpoint = config["AzureEndpoint"] ?? throw new InvalidOperationException("AzureEndpoint no configurado en User Secrets");

var credential = new AzureKeyCredential(key);

DocumentAnalysisClient client = new DocumentAnalysisClient(new Uri(endpoint), credential);

//Uri documentUri = new Uri("https://pablocominiello.github.io/assets/CBUdemo.jpg");
using var stream = new FileStream("./images/GaliciaChat.jpg", FileMode.Open);

//AnalyzeDocumentOperation operation = await client.AnalyzeDocumentFromUriAsync(WaitUntil.Completed,  "prebuilt-layout", documentUri);
AnalyzeDocumentOperation operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed,  "prebuilt-layout", stream);

AnalyzeResult result = operation.Value;
foreach (DocumentPage page in result.Pages)
{
    Console.WriteLine($"Page {page.PageNumber} has {page.Lines.Count} lines.");

    foreach (DocumentLine line in page.Lines)
    {
        Console.WriteLine($"  Line: '{line.Content}'");
    }
}

