
namespace Services
{
    public interface IOwnerService
    {
        Task<ObservableCollection<OwnerDto>> GetAll(CancellationToken cancellationToken = default);
        Task<OwnerDto> GetById(int id, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Create(OwnerCreateDto dto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Update(int id, OwnerUpdateDto dto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Delete(int id, CancellationToken cancellationToken = default);
    }
}
