
namespace Services
{
    public interface IServiceCenterService
    {
        Task<ObservableCollection<ServiceCenterDto>> GetAll(CancellationToken cancellationToken = default);
        Task<ServiceCenterDto> GetById(int id, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Create(ServiceCenterCreateDto dto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Update(int id, ServiceCenterUpdateDto dto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Delete(int id, CancellationToken cancellationToken = default);
    }
}
