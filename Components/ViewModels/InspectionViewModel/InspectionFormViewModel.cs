namespace ViewModels;

public class InspectionFormViewModel : ComponentBaseViewModel
{
    [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
    [Parameter] public InspectionCreateDto? InspectionCreate { get; set; }
    [Parameter] public InspectionUpdateDto? InspectionUpdate { get; set; }

    public async Task CreateOrUpdate()
    {
        var response = InspectionCreate != null
            ? await InspectionService!.Create(InspectionCreate)
            : await InspectionService!.Update(InspectionUpdate!.Id, InspectionUpdate!);

        Snackbar!.Add(response.IsSuccess ? "Success!" : "Error!", response.IsSuccess ? Severity.Success : Severity.Error);
        MudDialog?.Close(DialogResult.Ok(true));
    }

    public void Cancel() => MudDialog?.Cancel();

    public bool Disabled =>
        (InspectionCreate != null && string.IsNullOrWhiteSpace(InspectionCreate.Result)) ||
        (InspectionUpdate != null && string.IsNullOrWhiteSpace(InspectionUpdate.Result));
}
