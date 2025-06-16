namespace ViewModels;

public class OwnerFormViewModel : ComponentBaseViewModel
{
    [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }

    [Parameter] public OwnerCreateDto? OwnerCreate { get; set; }
    [Parameter] public OwnerUpdateDto? OwnerUpdate { get; set; }

    protected const string ValidationMessage = "Required.";

    public async Task CreateOrUpdate()
    {
        var response = OwnerCreate != null
            ? await OwnerService!.Create(OwnerCreate)
            : await OwnerService!.Update(OwnerUpdate!.Id, OwnerUpdate!);

        Snackbar!.Add(response.IsSuccess ? "Success!" : "Error!", response.IsSuccess ? Severity.Success : Severity.Error);
        MudDialog?.Close(DialogResult.Ok(true));
    }

    public void Cancel() => MudDialog?.Cancel();

    public bool Disabled =>
        (OwnerCreate != null && string.IsNullOrWhiteSpace(OwnerCreate.Email)) ||
        (OwnerUpdate != null && string.IsNullOrWhiteSpace(OwnerUpdate.Email));
}
