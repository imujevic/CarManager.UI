

namespace Services
{
    public class CarService : ICarService
    {
        private readonly IApiService _apiService;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public CarService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ObservableCollection<CarDto>> GetAll(CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Get($"{ApiEndpoints.CarController}");
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<ObservableCollection<CarDto>>(stream, _options, cancellationToken) ?? new();
        }

        public async Task<CarDto> GetById(int id, CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Get($"{ApiEndpoints.CarController}/details/{id}");
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<CarDto>(stream, _options, cancellationToken) ?? new();
        }

        public async Task<GeneralResponseDto> Create(CarCreateDto dto, CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Post($"{ApiEndpoints.CarController}/create", dto);
            return await DeserializeResponse(response, cancellationToken);
        }

        public async Task<GeneralResponseDto> Update(int id, CarUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Put($"{ApiEndpoints.CarController}/update/{id}", dto);
            return await DeserializeResponse(response, cancellationToken);
        }

        public async Task<GeneralResponseDto> Delete(int id, CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Delete($"{ApiEndpoints.CarController}/{id}");
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
