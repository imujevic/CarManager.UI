using Components.Dialog;
using Components.Entities.Owner;
using Components.Entities.Owners;
using Services.Common.Dto;
using System.Collections.ObjectModel;

namespace ViewModels;

public class OwnerListViewModel : ComponentBaseViewModel
{
    protected bool Loading;
    protected ObservableCollection<OwnerDto> Owners { get; set; } = new ObservableCollection<OwnerDto>();
    protected string? SearchOwnerName { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Loading = true;
        await LoadOwners();
        Loading = false;
    }

    protected async Task CreateOrUpdateOwner(OwnerDto ownerDto)
    {
        DialogParameters parameters;
        if (ownerDto.OwnerId == 0)
        {
            var ownerCreate = ownerDto.Adapt<OwnerCreateDto>();
            parameters = new DialogParameters { ["OwnerCreate"] = ownerCreate };
        }
        else
        {
            var ownerUpdate = ownerDto.Adapt<OwnerUpdateDto>();
            parameters = new DialogParameters { ["OwnerUpdate"] = ownerUpdate };
        }

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium };
        var dialogTitle = ownerDto.OwnerId == 0 ? "Create Owner" : "Update Owner";
        var dialog = await DialogService!.ShowAsync<OwnerFormComponent>(dialogTitle, parameters, options);

        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await LoadOwners();
        }
    }

    private async Task LoadOwners()
    {
        try
        {
            var owners = await OwnerService!.GetAll();
            Owners.Clear();
            foreach (var owner in owners)
            {
                Owners.Add(owner);
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error loading owners: {ex.Message}");
        }
    }

    protected async Task DeleteOwner(OwnerDto owner)
    {
        var parameters = new DialogParameters
        {
            { "ContentText", "Are you sure you want to delete this owner?" },
            { "ButtonText", "Delete" },
            { "Color", Color.Success }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
        var dialog = await DialogService!.ShowAsync<ConfirmComponent>("Delete Owner", parameters, options);
        var result = await dialog.Result;

        if (result!.Canceled)
        {
            return;
        }

        var response = await OwnerService!.Delete(owner.OwnerId);
        HandleResponse(response, owner);
    }

    protected bool FilterFunc(OwnerDto element)
    {
        return string.IsNullOrWhiteSpace(SearchOwnerName) ||
               element.FirstName!.Contains(SearchOwnerName, StringComparison.OrdinalIgnoreCase) ||
               element.LastName!.Contains(SearchOwnerName, StringComparison.OrdinalIgnoreCase);
    }

    private void HandleResponse(GeneralResponseDto response, OwnerDto owner)
    {
        if (response.IsSuccess)
        {
            Owners.Remove(owner);
            Snackbar!.Add("Success!", Severity.Success);
        }
        else
        {
            Snackbar!.Add("Error", Severity.Error);
        }
    }
}
