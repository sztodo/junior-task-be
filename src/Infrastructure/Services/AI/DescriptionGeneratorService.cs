using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.AI;

public class DescriptionGeneratorService : IDescriptionGeneratorService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<DescriptionGeneratorService> _logger;

    private const string Model = "gemini-2.5-flash";
    private const string ApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/";

    public DescriptionGeneratorService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<DescriptionGeneratorService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiKey = configuration["Gemini:ApiKey"]
            ?? throw new InvalidOperationException(
                "Gemini:ApiKey is not configured. " +
                "Add it to appsettings.Development.json or as an environment variable.");
    }

    public async Task<GenerateDescriptionResponse> GenerateAsync(
        GenerateDescriptionRequest request)
    {
        var prompt = BuildPrompt(request);

        // 1. Gemini specific structure (contents -> parts -> text)
        var payload = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[] { new { text = prompt } }
                }
            },
            generationConfig = new
            {
                maxOutputTokens = 500,
                temperature = 0.7 // Creativity control
            }
        };

        var json = JsonSerializer.Serialize(payload);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        // 2. AI request building
        var requestUrl = $"{ApiUrl}{Model}:generateContent?key={_apiKey}";

        _logger.LogInformation("Calling Gemini API for device: {Name}", request.Name);

        // 3. Sending request
        var response = await _httpClient.PostAsync(requestUrl, httpContent);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            _logger.LogError("Gemini API returned {Status}: {Body}", response.StatusCode, errorBody);

            throw new InvalidOperationException($"AI generation failed: {response.StatusCode}");
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        var description = ParseDescription(responseJson);

        return new GenerateDescriptionResponse(description);
    }

    private static string BuildPrompt(GenerateDescriptionRequest r) => $"""
        You are an AI feature that automatically creates human-readable, concise, and informative descriptions of devices based on their technical specifications.

        Turn the specs into **one very short, natural sentence**.

        Focus on: main use case, performance (processor + RAM) and OS.

        Output only the sentence. Make it sound premium but brief.
        Rules:
        - DO NOT include the device name.
        - Include the manufacturer and OS.
        - Focus on the business use case.
        - Output ONLY the plain text sentence.

        Examples:

        Input: Name - iPhone 17 Pro, Manufacturer - Apple, OS - iOS, Type - phone, RAM - 12GB, Processor - A19 Pro
        Output: “A high-performance Apple smartphone running iOS, suitable for daily business use.”

        Input: Name - Galaxy S25 Ultra, Manufacturer - Samsung, OS - Android, Type - phone, RAM - 16GB, Processor - Snapdragon 8 Elite
        Output: “The most powerful Android flagship for the 25th version with exceptional performance, ideal for photography and multitasking.”

        Input: Name - Pixel 9 Pro, Manufacturer - Google, OS - Android, Type - phone, RAM - 12GB, Processor - Tensor G4
        Output: “A smart Android phone running clean software, great for everyday productivity and casual photography.”

        Input: Name - MacBook Air M3, Manufacturer - Apple, OS - macOS, Type - laptop, RAM - 16GB, Processor - M3
        Output: “A lightweight Apple laptop with blazing performance, perfect for students and professionals.”

        Now do the same for this product:
        Input:
        - Name:             {r.Name}
        - Manufacturer:     {r.Manufacturer}
        - Type:             {r.Type}
        - Operating System: {r.OperatingSystem} {r.OsVersion}
        - Processor:        {r.Processor}
        - RAM:              {r.RamAmount} GB
        """;

    private static string ParseDescription(string responseJson)
    {
        using var doc = JsonDocument.Parse(responseJson);

        try
        {
            // Gemini structure: candidates[0] -> content -> parts[0] -> text
            return doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString()?.Trim() ?? throw new Exception();
        }
        catch
        {
            throw new InvalidOperationException("Gemini API returned an unexpected structure. Check if the prompt was blocked by safety filters.");
        }
    }
}
