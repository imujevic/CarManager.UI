using System.Collections.ObjectModel;

namespace ViewModels;

public class ServiceRecordListViewModel : ComponentBaseViewModel
{
    protected bool Loading;
    protected string? SearchText;

    public ObservableCollection<ServiceRecordDto> ServiceRecords { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadServiceRecords();
    }

    protected async Task LoadServiceRecords()
    {
        try
        {
            Loading = true;
            ServiceRecords = await ServiceRecordService!.GetAll();
        }
        catch (HttpRequestException ex)
        {
            Snackbar?.Add($"Failed to load records: {ex.Message}", Severity.Error);
        }
        finally
        {
            Loading = false;
        }
    }

    protected async Task Delete(ServiceRecordDto record)
    {
        var confirm = await DialogService!.ShowMessageBox(
            title: "Delete Confirmation",
            markupMessage: (MarkupString)$"Are you sure you want to delete this record?",
            yesText: "Delete", cancelText: "Cancel");

        if (confirm == true)
        {
            var response = await ServiceRecordService!.Delete(record.Id);
            if (response.IsSuccess)
            {
                ServiceRecords.Remove(record);
                Snackbar!.Add("Deleted successfully.", Severity.Success);
            }
            else
            {
                Snackbar!.Add("Failed to delete.", Severity.Error);
            }
        }
    }

    protected bool FilterFunc(ServiceRecordDto r) =>
        string.IsNullOrWhiteSpace(SearchText)
        || r.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
        || r.Cost.ToString().Contains(SearchText);
}
