using System.Collections.ObjectModel;

namespace ViewModels;

public class OwnerListViewModel : ComponentBaseViewModel
{
    protected ObservableCollection<OwnerDto> Owners { get; set; } = [];
    protected string? SearchText;
    protected bool Loading;

    protected override async Task OnInitializedAsync() => await Load();

    private async Task Load()
    {
        Loading = true;
        Owners = await OwnerService!.GetAll();
        Loading = false;
    }

    protected async Task Delete(OwnerDto owner)
    {
        var result = await DialogService!.ShowMessageBox("Confirm", "Delete this owner?", yesText: "Delete");
        if (result == true)
        {
            var res = await OwnerService!.Delete(owner.Id);
            if (res.IsSuccess)
            {
                Owners.Remove(owner);
                Snackbar!.Add("Deleted.", Severity.Success);
            }
        }
    }

    protected bool FilterFunc(OwnerDto o) =>
        string.IsNullOrWhiteSpace(SearchText) ||
        o.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
}
