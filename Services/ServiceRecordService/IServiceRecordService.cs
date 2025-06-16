

namespace Services
{
    public interface IServiceRecordService
    {
        Task<ObservableCollection<ServiceRecordDto>> GetAll(CancellationToken cancellationToken = default);
        Task<ServiceRecordDto> GetById(int id, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Create(ServiceRecordCreateDto dto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Update(int id, ServiceRecordUpdateDto dto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Delete(int id, CancellationToken cancellationToken = default);
    }
}
