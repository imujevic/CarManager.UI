namespace ViewModels;

public class ServiceCenterFormViewModel : ComponentBaseViewModel
{
    [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
    [Parameter] public ServiceCenterCreateDto? ServiceCenterCreate { get; set; }
    [Parameter] public ServiceCenterUpdateDto? ServiceCenterUpdate { get; set; }

    public async Task CreateOrUpdate()
    {
        var response = ServiceCenterCreate != null
            ? await ServiceCenterService!.Create(ServiceCenterCreate)
            : await ServiceCenterService!.Update(ServiceCenterUpdate!.Id, ServiceCenterUpdate!);

        Snackbar!.Add(response.IsSuccess ? "Success!" : "Error!", response.IsSuccess ? Severity.Success : Severity.Error);
        MudDialog?.Close(DialogResult.Ok(true));
    }

    public void Cancel() => MudDialog?.Cancel();

    public bool Disabled =>
        (ServiceCenterCreate != null && string.IsNullOrWhiteSpace(ServiceCenterCreate.Name)) ||
        (ServiceCenterUpdate != null && string.IsNullOrWhiteSpace(ServiceCenterUpdate.Name));
}
