

namespace ViewModels;

public class ServiceRecordFormViewModel : ComponentBaseViewModel
{
    [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }

    [Parameter] public ServiceRecordCreateDto? ServiceRecordCreate { get; set; }
    [Parameter] public ServiceRecordUpdateDto? ServiceRecordUpdate { get; set; }

    public bool IsEditMode => ServiceRecordUpdate != null;

    protected override async Task OnInitializedAsync()
    {
        await Task.CompletedTask;
    }

    public async Task CreateOrUpdate()
    {
        GeneralResponseDto response;

        try
        {
            if (!IsEditMode && ServiceRecordCreate != null)
            {
                response = await ServiceRecordService!.Create(ServiceRecordCreate);
            }
            else if (IsEditMode && ServiceRecordUpdate != null)
            {
                response = await ServiceRecordService!.Update(ServiceRecordUpdate.Id, ServiceRecordUpdate);
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
            string.IsNullOrWhiteSpace(ServiceRecordCreate?.Description) ||
            ServiceRecordCreate?.Cost <= 0 ||
            ServiceRecordCreate?.ServiceDate == null))
        ||
        (IsEditMode && (
            string.IsNullOrWhiteSpace(ServiceRecordUpdate?.Description) ||
            ServiceRecordUpdate?.Cost <= 0 ||
            ServiceRecordUpdate?.ServiceDate == null));
}
