using System.Collections.ObjectModel;

namespace ViewModels;

public class ServiceRecordListViewModel : ComponentBaseViewModel
{
    protected ObservableCollection<ServiceRecordDto> Records { get; set; } = [];
    protected string? SearchText;
    protected bool Loading;

    protected override async Task OnInitializedAsync() => await Load();

    private async Task Load()
    {
        Loading = true;
        Records = await ServiceRecordService!.GetAll();
        Loading = false;
    }

    protected async Task Delete(ServiceRecordDto record)
    {
        var result = await DialogService!.ShowMessageBox("Confirm", "Delete this service record?", yesText: "Delete");
        if (result == true)
        {
            var res = await ServiceRecordService!.Delete(record.Id);
            if (res.IsSuccess)
            {
                Records.Remove(record);
                Snackbar!.Add("Deleted.", Severity.Success);
            }
        }
    }

    protected bool FilterFunc(ServiceRecordDto r) =>
        string.IsNullOrWhiteSpace(SearchText) ||
        r.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
}
