using Dto;

namespace ViewModels;

public class ServiceCenterFormViewModel : ComponentBaseViewModel
{
    [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }

    [Parameter] public ServiceCenterCreateDto? ServiceCenterCreate { get; set; }
    [Parameter] public ServiceCenterUpdateDto? ServiceCenterUpdate { get; set; }

    public bool IsEditMode => ServiceCenterUpdate != null;

    protected override async Task OnInitializedAsync()
    {
        await Task.CompletedTask;
    }

    public async Task CreateOrUpdate()
    {
        GeneralResponseDto response;

        try
        {
            if (!IsEditMode && ServiceCenterCreate != null)
            {
                response = await ServiceCenterService!.Create(ServiceCenterCreate);
            }
            else if (IsEditMode && ServiceCenterUpdate != null)
            {
                response = await ServiceCenterService!.Update(ServiceCenterUpdate.Id, ServiceCenterUpdate);
            }
            else
            {
                Snackbar?.Add("Invalid form data", Severity.Error);
                return;
            }

            Snackbar!.Add(response.IsSuccess ? "Success!" : "Error!",
                response.IsSuccess ? Severity.Success : Severity.Error);

            MudDialog?.Close(DialogResult.Ok(true));
        }
        catch (HttpRequestException ex)
        {
            Snackbar?.Add($"Network error: {ex.Message}", Severity.Error);
            MudDialog?.Close(DialogResult.Ok(true));
        }
    }

    public void Cancel() => MudDialog?.Cancel();

    public bool Disabled =>
        (!IsEditMode && (
            string.IsNullOrWhiteSpace(ServiceCenterCreate?.Name) ||
            string.IsNullOrWhiteSpace(ServiceCenterCreate?.Address) ||
            string.IsNullOrWhiteSpace(ServiceCenterCreate?.PhoneNumber)))
        ||
        (IsEditMode && (
            string.IsNullOrWhiteSpace(ServiceCenterUpdate?.Name) ||
            string.IsNullOrWhiteSpace(ServiceCenterUpdate?.Address) ||
            string.IsNullOrWhiteSpace(ServiceCenterUpdate?.PhoneNumber)));
}
