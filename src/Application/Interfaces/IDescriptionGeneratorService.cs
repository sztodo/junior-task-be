using Application.Dtos;

namespace Application.Interfaces;

public interface IDescriptionGeneratorService
{
    Task<GenerateDescriptionResponse> GenerateAsync(GenerateDescriptionRequest request);
}
