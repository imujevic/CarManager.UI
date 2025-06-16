
namespace Services
{
    public interface IBookingService
    {
        Task<ObservableCollection<BookingDto>> GetAll(CancellationToken cancellationToken = default);
        Task<BookingDto> GetById(int id, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Create(BookingCreateDto dto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Update(int id, BookingUpdateDto dto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto> Delete(int id, CancellationToken cancellationToken = default);
    }
}
