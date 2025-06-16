using System.Collections.ObjectModel;

namespace ViewModels;

public class CarListViewModel : ComponentBaseViewModel
{
    protected ObservableCollection<CarDto> Cars { get; set; } = [];
    protected string? SearchText;
    protected bool Loading;

    protected override async Task OnInitializedAsync()
    {
        await LoadCars();
    }

    private async Task LoadCars()
    {
        Loading = true;
        Cars = await CarService!.GetAll();
        Loading = false;
        StateHasChanged();
    }

    protected async Task Delete(CarDto car)
    {
        var result = await DialogService!.ShowMessageBox("Confirm", "Delete this car?", yesText: "Delete");
        if (result == true)
        {
            var res = await CarService.Delete(car.Id);
            if (res.IsSuccess)
            {
                Cars.Remove(car);
                Snackbar!.Add("Deleted.", Severity.Success);
            }
        }
    }

    protected bool FilterFunc(CarDto c) =>
        string.IsNullOrWhiteSpace(SearchText) ||
        c.Model.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
}
