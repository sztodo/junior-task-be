using Application.Dtos;

namespace Application.Interfaces;

public interface ISearchService
{
    Task<IEnumerable<DeviceSearchResultDto>> SearchAsync(string query);

}
