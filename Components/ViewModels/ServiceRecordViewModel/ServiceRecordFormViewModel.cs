namespace ViewModels;

public class ServiceRecordFormViewModel : ComponentBaseViewModel
{
    [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
    [Parameter] public ServiceRecordCreateDto? ServiceRecordCreate { get; set; }
    [Parameter] public ServiceRecordUpdateDto? ServiceRecordUpdate { get; set; }

    public async Task CreateOrUpdate()
    {
        var response = ServiceRecordCreate != null
            ? await ServiceRecordService!.Create(ServiceRecordCreate)
            : await ServiceRecordService!.Update(ServiceRecordUpdate!.Id, ServiceRecordUpdate!);

        Snackbar!.Add(response.IsSuccess ? "Success!" : "Error!", response.IsSuccess ? Severity.Success : Severity.Error);
        MudDialog?.Close(DialogResult.Ok(true));
    }

    public void Cancel() => MudDialog?.Cancel();

    public bool Disabled =>
        (ServiceRecordCreate != null && string.IsNullOrWhiteSpace(ServiceRecordCreate.Description)) ||
        (ServiceRecordUpdate != null && string.IsNullOrWhiteSpace(ServiceRecordUpdate.Description));
}
