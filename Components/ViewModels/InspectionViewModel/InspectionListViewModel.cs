using System.Collections.ObjectModel;

namespace ViewModels;

public class InspectionListViewModel : ComponentBaseViewModel
{
    protected ObservableCollection<InspectionDto> Inspections { get; set; } = [];
    protected string? SearchText;
    protected bool Loading;

    protected override async Task OnInitializedAsync() => await Load();

    private async Task Load()
    {
        Loading = true;
        Inspections = await InspectionService!.GetAll();
        Loading = false;
    }

    protected async Task Delete(InspectionDto inspection)
    {
        var result = await DialogService!.ShowMessageBox("Confirm", "Delete this inspection?", yesText: "Delete");
        if (result == true)
        {
            var res = await InspectionService!.Delete(inspection.Id);
            if (res.IsSuccess)
            {
                Inspections.Remove(inspection);
                Snackbar!.Add("Deleted.", Severity.Success);
            }
        }
    }

    protected bool FilterFunc(InspectionDto i) =>
        string.IsNullOrWhiteSpace(SearchText) ||
        i.Result.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
}
