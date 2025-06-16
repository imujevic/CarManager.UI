namespace ViewModels;

public class CarFormViewModel : ComponentBaseViewModel
{
    [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
    [Parameter] public CarCreateDto? CarCreate { get; set; }
    [Parameter] public CarUpdateDto? CarUpdate { get; set; }

    protected const string ValidationMessage = "Required.";

    public async Task CreateOrUpdate()
    {
        var response = CarCreate != null
            ? await CarService!.Create(CarCreate)
            : await CarService!.Update(CarUpdate!.Id, CarUpdate!);

        Snackbar!.Add(response.IsSuccess ? "Success!" : "Error!", response.IsSuccess ? Severity.Success : Severity.Error);
        MudDialog?.Close(DialogResult.Ok(true));
    }

    public void Cancel() => MudDialog?.Cancel();

    public bool Disabled =>
        (CarCreate != null && string.IsNullOrWhiteSpace(CarCreate.Model)) ||
        (CarUpdate != null && string.IsNullOrWhiteSpace(CarUpdate.Model));
}
