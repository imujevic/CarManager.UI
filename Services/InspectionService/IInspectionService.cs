
namespace Services
{
    public interface IInspectionService
    {
        Task<ObservableCollection<InspectionDto>> GetAll(CancellationToken cancellationToken = default);
        Task<InspectionDto> GetById(int id, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Create(InspectionCreateDto dto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Update(int id, InspectionUpdateDto dto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Delete(int id, CancellationToken cancellationToken = default);
    }
}
