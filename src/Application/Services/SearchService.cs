using Application.Dtos;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services;

public class SearchService : ISearchService
{
    private readonly IDeviceRepository _deviceRepository;

    private const int NameWeight = 40;
    private const int ManufacturerWeight = 30;
    private const int ProcessorWeight = 20;
    private const int RamWeight = 10;
    private const int OsVersionWeight = 10;
    private const int OsWeight = 10;

    public SearchService(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public async Task<IEnumerable<DeviceSearchResultDto>> SearchAsync(string query)
    {
        var tokens = Tokenize(query);

        if (tokens.Length == 0)
            return Enumerable.Empty<DeviceSearchResultDto>();

        var devices = await _deviceRepository.GetAllWithUsersAsync();

        var results = devices
            .Select(device =>
            {
                var score = tokens.Sum(token => ScoreDevice(device, token));
                return (device, score);
            })
            .Where(x => x.score > 0)
            .OrderByDescending(x => x.score)
            .Select(x => new DeviceSearchResultDto(
                x.device.Id,
                x.device.Name,
                x.device.Manufacturer,
                x.device.Type.ToString(),
                x.device.OperatingSystem,
                x.device.OsVersion,
                x.device.Processor,
                x.device.RamAmount,
                x.device.Description,
                x.device.AssignedUserId,
                x.device.AssignedUser?.Name,
                x.score))
            .ToList();

        return results;
    }

    private static string[] Tokenize(string query)
        => query
            .ToLowerInvariant()
            .Replace(",", " ").Replace(".", " ").Replace("-", " ")
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(t => t.Length > 0)
            .ToArray();

    private static int ScoreDevice(Device device, string token)
    {
        var score = 0;

        if (device.Name.ToLowerInvariant().Contains(token))
            score += NameWeight;

        if (device.Manufacturer.ToLowerInvariant().Contains(token))
            score += ManufacturerWeight;

        if (device.Processor.ToLowerInvariant().Contains(token))
            score += ProcessorWeight;

        if (device.RamAmount.ToString().Contains(token))
            score += RamWeight;
        if (device.OsVersion.ToString().Contains(token))
            score += OsVersionWeight;
        if (device.OperatingSystem.ToString().Contains(token))
            score += OsWeight;
        return score;
    }
}
