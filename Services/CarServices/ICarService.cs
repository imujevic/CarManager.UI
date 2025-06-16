

namespace Services
{
    public interface ICarService
    {
        Task<ObservableCollection<CarDto>> GetAll(CancellationToken cancellationToken = default);
        Task<CarDto> GetById(int id, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Create(CarCreateDto dto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Update(int id, CarUpdateDto dto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Delete(int id, CancellationToken cancellationToken = default);
    }
}
