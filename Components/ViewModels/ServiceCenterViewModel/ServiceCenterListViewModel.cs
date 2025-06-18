using System.Collections.ObjectModel;
using Dto;

namespace ViewModels;

public class ServiceCenterListViewModel : ComponentBaseViewModel
{
    protected bool Loading;
    protected string? SearchText;

    public ObservableCollection<ServiceCenterDto> ServiceCenters { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadServiceCenters();
    }

    protected async Task LoadServiceCenters()
    {
        try
        {
            Loading = true;
            ServiceCenters = await ServiceCenterService!.GetAll();
        }
        catch (HttpRequestException ex)
        {
            Snackbar?.Add($"Failed to load: {ex.Message}", Severity.Error);
        }
        finally
        {
            Loading = false;
        }
    }

    protected async Task Delete(ServiceCenterDto serviceCenter)
    {
        var confirm = await DialogService!.ShowMessageBox(
            title: "Delete Confirmation",
            markupMessage: (MarkupString)$"Are you sure you want to delete <b>{serviceCenter.Name}</b>?",
            yesText: "Delete", cancelText: "Cancel");

        if (confirm == true)
        {
            var response = await ServiceCenterService!.Delete(serviceCenter.Id);
            if (response.IsSuccess)
            {
                ServiceCenters.Remove(serviceCenter);
                Snackbar!.Add("Deleted successfully.", Severity.Success);
            }
            else
            {
                Snackbar!.Add("Failed to delete.", Severity.Error);
            }
        }
    }

    protected bool FilterFunc(ServiceCenterDto sc)
    {
        return string.IsNullOrWhiteSpace(SearchText)
               || sc.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
               || sc.Address.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
               || sc.PhoneNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
    }
}
