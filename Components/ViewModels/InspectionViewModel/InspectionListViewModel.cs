using System.Collections.ObjectModel;

namespace ViewModels;

public class InspectionListViewModel : ComponentBaseViewModel
{
    protected ObservableCollection<InspectionDto> Inspections { get; set; } = [];
    protected string? SearchText;
    protected bool Loading;

    protected override async Task OnInitializedAsync()
    {
        await Load();
    }

    private async Task Load()
    {
        try
        {
            Loading = true;
            Inspections = await InspectionService!.GetAll();
        }
        catch (HttpRequestException ex)
        {
            Snackbar?.Add($"Error loading inspections: {ex.Message}", Severity.Error);
        }
        finally
        {
            Loading = false;
        }
    }

    protected async Task Delete(InspectionDto inspection)
    {
        var result = await DialogService!.ShowMessageBox(
            title: "Confirm",
            markupMessage: (MarkupString)"Are you sure you want to delete this inspection?",
            yesText: "Delete", noText: "Cancel");

        if (result == true)
        {
            var response = await InspectionService!.Delete(inspection.Id);
            if (response.IsSuccess)
            {
                Inspections.Remove(inspection);
                Snackbar!.Add("Deleted successfully.", Severity.Success);
            }
            else
            {
                Snackbar!.Add("Deletion failed.", Severity.Error);
            }
        }
    }

    protected bool FilterFunc(InspectionDto inspection)
    {
        return string.IsNullOrWhiteSpace(SearchText)
               || inspection.Result.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
    }
}
