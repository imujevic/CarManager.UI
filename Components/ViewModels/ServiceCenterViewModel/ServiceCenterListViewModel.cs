using System.Collections.ObjectModel;

namespace ViewModels;

public class ServiceCenterListViewModel : ComponentBaseViewModel
{
    protected ObservableCollection<ServiceCenterDto> Centers { get; set; } = [];
    protected string? SearchText;
    protected bool Loading;

    protected override async Task OnInitializedAsync() => await Load();

    private async Task Load()
    {
        Loading = true;
        Centers = await ServiceCenterService!.GetAll();
        Loading = false;
    }

    protected async Task Delete(ServiceCenterDto center)
    {
        var result = await DialogService!.ShowMessageBox("Confirm", "Delete this service center?", yesText: "Delete");
        if (result == true)
        {
            var res = await ServiceCenterService!.Delete(center.Id);
            if (res.IsSuccess)
            {
                Centers.Remove(center);
                Snackbar!.Add("Deleted.", Severity.Success);
            }
        }
    }

    protected bool FilterFunc(ServiceCenterDto c) =>
        string.IsNullOrWhiteSpace(SearchText) ||
        c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
}
