

namespace Services
{
    public class BookingService : IBookingService
    {
        private readonly IApiService _apiService;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public BookingService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ObservableCollection<BookingDto>> GetAll(CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Get($"{ApiEndpoints.BookingController}");
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<ObservableCollection<BookingDto>>(stream, _options, cancellationToken) ?? new();
        }

        public async Task<BookingDto> GetById(int id, CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Get($"{ApiEndpoints.BookingController}/details/{id}");
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<BookingDto>(stream, _options, cancellationToken) ?? new();
        }

        public async Task<GeneralResponseDto> Create(BookingCreateDto dto, CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Post($"{ApiEndpoints.BookingController}/create", dto);
            return await DeserializeResponse(response, cancellationToken);
        }

        public async Task<GeneralResponseDto> Update(int id, BookingUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Put($"{ApiEndpoints.BookingController}/update/{id}", dto);
            return await DeserializeResponse(response, cancellationToken);
        }

        public async Task<GeneralResponseDto> Delete(int id, CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Delete($"{ApiEndpoints.BookingController}/{id}");
            return new GeneralResponseDto { IsSuccess = response.IsSuccessStatusCode };
        }

        private static async Task<GeneralResponseDto> DeserializeResponse(HttpResponseMessage response, CancellationToken token)
        {
            if (!response.IsSuccessStatusCode) return new GeneralResponseDto { IsSuccess = false };
            var stream = await response.Content.ReadAsStreamAsync(token);
            return await JsonSerializer.DeserializeAsync<GeneralResponseDto>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, token) ?? new();
        }
    }
}
