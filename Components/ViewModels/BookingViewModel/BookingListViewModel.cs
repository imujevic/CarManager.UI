using System.Collections.ObjectModel;

namespace ViewModels;

public class BookingListViewModel : ComponentBaseViewModel
{
    protected ObservableCollection<BookingDto> Bookings { get; set; } = [];
    protected string? SearchText;
    protected bool Loading;

    protected override async Task OnInitializedAsync() => await Load();

    private async Task Load()
    {
        Loading = true;
        Bookings = await BookingService!.GetAll();
        Loading = false;
    }

    protected async Task Delete(BookingDto booking)
    {
        var result = await DialogService!.ShowMessageBox("Confirm", "Delete this booking?", yesText: "Delete");
        if (result == true)
        {
            var res = await BookingService!.Delete(booking.Id);
            if (res.IsSuccess)
            {
                Bookings.Remove(booking);
                Snackbar!.Add("Deleted.", Severity.Success);
            }
        }
    }

    protected bool FilterFunc(BookingDto b) =>
        string.IsNullOrWhiteSpace(SearchText) ||
        b.BookingDate.ToShortDateString().Contains(SearchText, StringComparison.OrdinalIgnoreCase);
}
