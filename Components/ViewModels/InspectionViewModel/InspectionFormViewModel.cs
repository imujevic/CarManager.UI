namespace ViewModels;

public class InspectionFormViewModel : ComponentBaseViewModel
{
    [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }

    [Parameter] public InspectionCreateDto? InspectionCreate { get; set; }
    [Parameter] public InspectionUpdateDto? InspectionUpdate { get; set; }

    protected const string ValidationMessage = "Field is required.";

    public async Task CreateOrUpdate()
    {
        GeneralResponseDto response;

        if (InspectionCreate != null)
        {
            response = await InspectionService!.Create(InspectionCreate);
        }
        else if (InspectionUpdate != null)
        {
            response = await InspectionService!.Update(InspectionUpdate.Id, InspectionUpdate);
        }
        else
        {
            Snackbar!.Add("Invalid data", Severity.Error);
            return;
        }

        Snackbar!.Add(response.IsSuccess ? "Success!" : "Error!",
                      response.IsSuccess ? Severity.Success : Severity.Error);

        MudDialog?.Close(DialogResult.Ok(true));
    }

    public void Cancel() => MudDialog?.Cancel();

    public bool Disabled =>
        (InspectionCreate != null &&
         (string.IsNullOrWhiteSpace(InspectionCreate.Result) || InspectionCreate.InspectionDate == null)) ||
        (InspectionUpdate != null &&
         (string.IsNullOrWhiteSpace(InspectionUpdate.Result) || InspectionUpdate.InspectionDate == null));
}
