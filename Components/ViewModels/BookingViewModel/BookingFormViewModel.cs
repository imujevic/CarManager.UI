

namespace ViewModels
{
    public class BookingFormViewModel : ComponentBaseViewModel
    {
        [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }

        // Ako je Create, ovo će biti ne‑null; inače za Update
        [Parameter] public BookingCreateDto? BookingCreate { get; set; }
        [Parameter] public BookingUpdateDto? BookingUpdate { get; set; }

        protected override void OnInitialized()
        {
            // Inicijalizuj default vrednosti tako da DateTime nije MinValue
            if (BookingCreate != null && BookingCreate.BookingDate == default)
                BookingCreate.BookingDate = DateTime.Today;
            else if (BookingUpdate != null && BookingUpdate.BookingDate == default)
                BookingUpdate.BookingDate = DateTime.Today;
        }

        public bool Disabled =>
            (BookingCreate != null && (
                BookingCreate.CarId <= 0 ||
                BookingCreate.OwnerId <= 0 ||
                BookingCreate.ServiceCenterId <= 0))
            ||
            (BookingUpdate != null && (
                BookingUpdate.CarId <= 0 ||
                BookingUpdate.OwnerId <= 0 ||
                BookingUpdate.ServiceCenterId <= 0));

        public async Task CreateOrUpdate()
        {
            try
            {
                GeneralResponseDto response;

                if (BookingCreate != null)
                {
                    response = await BookingService!.Create(BookingCreate);
                }
                else if (BookingUpdate != null)
                {
                    response = await BookingService!.Update(BookingUpdate.Id, BookingUpdate);
                }
                else
                {
                    Snackbar?.Add("No data to save.", Severity.Warning);
                    return;
                }

                if (response.IsSuccess)
                    Snackbar?.Add("Saved successfully!", Severity.Success);
                else
                    Snackbar?.Add("Operation failed.", Severity.Error);

                MudDialog?.Close(DialogResult.Ok(true));
            }
            catch (Exception ex)
            {
                Snackbar?.Add($"Error: {ex.Message}", Severity.Error);
                MudDialog?.Close(DialogResult.Ok(false));
            }
        }

        public void Cancel() => MudDialog?.Cancel();
    }
}
