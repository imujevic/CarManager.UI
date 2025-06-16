

namespace Services
{
    public class ServiceCenterService : IServiceCenterService
    {
        private readonly IApiService _apiService;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public ServiceCenterService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ObservableCollection<ServiceCenterDto>> GetAll(CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Get($"{ApiEndpoints.ServiceCenterController}");
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<ObservableCollection<ServiceCenterDto>>(stream, _options, cancellationToken) ?? new();
        }

        public async Task<ServiceCenterDto> GetById(int id, CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Get($"{ApiEndpoints.ServiceCenterController}/details/{id}");
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<ServiceCenterDto>(stream, _options, cancellationToken) ?? new();
        }

        public async Task<GeneralResponseDto> Create(ServiceCenterCreateDto dto, CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Post($"{ApiEndpoints.ServiceCenterController}/create", dto);
            return await DeserializeResponse(response, cancellationToken);
        }

        public async Task<GeneralResponseDto> Update(int id, ServiceCenterUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Put($"{ApiEndpoints.ServiceCenterController}/update/{id}", dto);
            return await DeserializeResponse(response, cancellationToken);
        }

        public async Task<GeneralResponseDto> Delete(int id, CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Delete($"{ApiEndpoints.ServiceCenterController}/{id}");
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
