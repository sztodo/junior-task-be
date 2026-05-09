using Application.Dtos;

namespace Application.Interfaces;

public interface ISearchService
{
    Task<IEnumerable<DeviceDto>> SearchAsync(string query);

}
