using Components.Dialog;
using Components.Entities.Car;
using Components.Entities.Cars;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Services.Common.Dto;
using System.Collections.ObjectModel;
using static MudBlazor.CategoryTypes;
namespace ViewModels;

public class CarListViewModel : ComponentBaseViewModel
{
    protected bool Loading;
    protected ObservableCollection<CarDto> Cars { get; set; } = new ObservableCollection<CarDto>();
    protected string? SearchCarModel { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Loading = true;
        await LoadCars();
        Loading = false;
    }

    protected async Task CreateOrUpdateCar(CarDto carDto)
    {
        DialogParameters parameters;
        if (carDto.CarId == 0)
        {
            var carCreate = carDto.Adapt<CarCreateDto>();
            parameters = new DialogParameters { ["CarCreate"] = carCreate };
        }
        else
        {
            var carUpdate = carDto.Adapt<CarUpdateDto>();
            parameters = new DialogParameters { ["CarUpdate"] = carUpdate };
        }

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium };
        var dialogTitle = carDto.CarId == 0 ? "Create Car" : "Update Car";
        var dialog = await DialogService!.ShowAsync<CarFormComponent>(dialogTitle, parameters, options);

        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await LoadCars();
        }
    }

    private async Task LoadCars()
    {
        try
        {
            var cars = await CarService!.GetAll();
            Cars.Clear();
            foreach (var car in cars)
            {
                Cars.Add(car);
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error loading cars: {ex.Message}");
        }
    }

    protected async Task DeleteCar(CarDto car)
    {
        var parameters = new DialogParameters
        {
            { "ContentText", "Are you sure you want to delete this car?" },
            { "ButtonText", "Delete" },
            { "Color", Color.Success }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
        var dialog = await DialogService!.ShowAsync<ConfirmComponent>("Delete Car", parameters, options);
        var result = await dialog.Result;

        if (result!.Canceled)
        {
            return;
        }

        var response = await CarService!.Delete(car.CarId);
        HandleResponse(response, car);
    }

    protected bool FilterFunc(CarDto element)
    {
        return string.IsNullOrWhiteSpace(SearchCarModel) ||
               element.Model!.Contains(SearchCarModel, StringComparison.OrdinalIgnoreCase) ||
               element.Brand!.Contains(SearchCarModel, StringComparison.OrdinalIgnoreCase);
    }

    private void HandleResponse(GeneralResponseDto response, CarDto car)
    {
        if (response.IsSuccess)
        {
            Cars.Remove(car);
            Snackbar!.Add("Success!", Severity.Success);
        }
        else
        {
            Snackbar!.Add("Error", Severity.Error);
        }
    }
}
