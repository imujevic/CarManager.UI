

namespace Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IApiService _apiService;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public OwnerService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ObservableCollection<OwnerDto>> GetAll(CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Get($"{ApiEndpoints.OwnerController}");
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<ObservableCollection<OwnerDto>>(stream, _options, cancellationToken) ?? new();
        }

        public async Task<OwnerDto> GetById(int id, CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Get($"{ApiEndpoints.OwnerController}/details/{id}");
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<OwnerDto>(stream, _options, cancellationToken) ?? new();
        }

        public async Task<GeneralResponseDto> Create(OwnerCreateDto dto, CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Post($"{ApiEndpoints.OwnerController}/create", dto);
            return await DeserializeResponse(response, cancellationToken);
        }

        public async Task<GeneralResponseDto> Update(int id, OwnerUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Put($"{ApiEndpoints.OwnerController}/update/{id}", dto);
            return await DeserializeResponse(response, cancellationToken);
        }

        public async Task<GeneralResponseDto> Delete(int id, CancellationToken cancellationToken = default)
        {
            var response = await _apiService.Delete($"{ApiEndpoints.OwnerController}/{id}");
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
