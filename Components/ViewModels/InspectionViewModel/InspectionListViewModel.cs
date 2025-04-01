using Components.Dialog;
using Components.Entities.Inspection;
using Components.Entities.Inspections;
using Services.Common.Dto;
using System.Collections.ObjectModel;

namespace ViewModels;

public class InspectionListViewModel : ComponentBaseViewModel
{
    protected bool Loading;
    protected ObservableCollection<InspectionDto> Inspections { get; set; } = new ObservableCollection<InspectionDto>();
    protected string? SearchInspection { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Loading = true;
        await LoadInspections();
        Loading = false;
    }

    protected async Task CreateOrUpdateInspection(InspectionDto inspectionDto)
    {
        DialogParameters parameters;
        if (inspectionDto.InspectionId == 0)
        {
            var inspectionCreate = inspectionDto.Adapt<InspectionCreateDto>();
            parameters = new DialogParameters { ["InspectionCreate"] = inspectionCreate };
        }
        else
        {
            var inspectionUpdate = inspectionDto.Adapt<InspectionUpdateDto>();
            parameters = new DialogParameters { ["InspectionUpdate"] = inspectionUpdate };
        }

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium };
        var dialogTitle = inspectionDto.InspectionId == 0 ? "Create Inspection" : "Update Inspection";
        var dialog = await DialogService!.ShowAsync<InspectionFormComponent>(dialogTitle, parameters, options);

        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await LoadInspections();
        }
    }

    private async Task LoadInspections()
    {
        try
        {
            var inspections = await InspectionService!.GetAll();
            Inspections.Clear();
            foreach (var inspection in inspections)
            {
                Inspections.Add(inspection);
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error loading inspections: {ex.Message}");
        }
    }

    protected async Task DeleteInspection(InspectionDto inspection)
    {
        var parameters = new DialogParameters
        {
            { "ContentText", "Are you sure you want to delete this inspection?" },
            { "ButtonText", "Delete" },
            { "Color", Color.Success }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
        var dialog = await DialogService!.ShowAsync<ConfirmComponent>("Delete Inspection", parameters, options);
        var result = await dialog.Result;

        if (result!.Canceled)
        {
            return;
        }

        var response = await InspectionService!.Delete(inspection.InspectionId);
        HandleResponse(response, inspection);
    }

    protected bool FilterFunc(InspectionDto element)
    {
        return string.IsNullOrWhiteSpace(SearchInspection) ||
               element.InspectionType!.Contains(SearchInspection, StringComparison.OrdinalIgnoreCase) ||
               element.CarId.ToString().Contains(SearchInspection);
    }

    private void HandleResponse(GeneralResponseDto response, InspectionDto inspection)
    {
        if (response.IsSuccess)
        {
            Inspections.Remove(inspection);
            Snackbar!.Add("Success!", Severity.Success);
        }
        else
        {
            Snackbar!.Add("Error", Severity.Error);
        }
    }
}
