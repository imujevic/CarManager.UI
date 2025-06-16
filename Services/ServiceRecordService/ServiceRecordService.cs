

namespace Services
{
    public class ServiceRecordService : IServiceRecordService
    {
        private readonly IApiService _apiService;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public ServiceRecordService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ObservableCollection<ServiceRecordDto>> GetAll(CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Get($"{ApiEndpoints.ServiceRecordController}");
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<ObservableCollection<ServiceRecordDto>>(stream, _options, cancellationToken) ?? new();
        }

        public async Task<ServiceRecordDto> GetById(int id, CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Get($"{ApiEndpoints.ServiceRecordController}/details/{id}");
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<ServiceRecordDto>(stream, _options, cancellationToken) ?? new();
        }

        public async Task<GeneralResponseDto> Create(ServiceRecordCreateDto dto, CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Post($"{ApiEndpoints.ServiceRecordController}/create", dto);
            return await DeserializeResponse(response, cancellationToken);
        }

        public async Task<GeneralResponseDto> Update(int id, ServiceRecordUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Put($"{ApiEndpoints.ServiceRecordController}/update/{id}", dto);
            return await DeserializeResponse(response, cancellationToken);
        }

        public async Task<GeneralResponseDto> Delete(int id, CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Delete($"{ApiEndpoints.ServiceRecordController}/{id}");
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
