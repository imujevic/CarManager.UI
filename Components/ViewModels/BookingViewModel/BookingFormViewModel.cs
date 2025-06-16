namespace ViewModels;

public class BookingFormViewModel : ComponentBaseViewModel
{
    [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
    [Parameter] public BookingCreateDto? BookingCreate { get; set; }
    [Parameter] public BookingUpdateDto? BookingUpdate { get; set; }

    public async Task CreateOrUpdate()
    {
        var response = BookingCreate != null
            ? await BookingService!.Create(BookingCreate)
            : await BookingService!.Update(BookingUpdate!.Id, BookingUpdate!);

        Snackbar!.Add(response.IsSuccess ? "Success!" : "Error!", response.IsSuccess ? Severity.Success : Severity.Error);
        MudDialog?.Close(DialogResult.Ok(true));
    }

    public void Cancel() => MudDialog?.Cancel();

    public bool Disabled =>
        (BookingCreate != null && BookingCreate.CarId == 0) ||
        (BookingUpdate != null && BookingUpdate.CarId == 0);
}
