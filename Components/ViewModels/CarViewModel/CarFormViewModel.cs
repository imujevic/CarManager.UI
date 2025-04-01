using Microsoft.AspNetCore.Cors.Infrastructure;
using Services.Common.Dto;

namespace ViewModels
{
    public class CarFormViewModel : ComponentBaseViewModel
    {
        [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
        protected const string ValidationMessage = "Field is required.";

        [Parameter]
        public CarCreateDto? CarCreate { get; set; }

        [Parameter]
        public CarUpdateDto? CarUpdate { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (CarUpdate != null)
            {
                StateHasChanged();
            }
        }

        public async Task CreateOrUpdate()
        {
            try
            {
                var response = new GeneralResponseDto();
                if (CarCreate != null && CarCreate.CarId == 0)
                {
                    response = await CarService!.Create(CarCreate);
                }
                else
                {
                    response = await CarService!.Update(CarUpdate!.CarId, CarUpdate);
                }

                HandleResponse(response);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                MudDialog!.Close(DialogResult.Ok(true));
            }
        }

        private void HandleResponse(GeneralResponseDto response)
        {
            var isSuccess = response?.IsSuccess == true;
            Snackbar!.Add(isSuccess ? "Success!" : "Error!", isSuccess ? Severity.Success : Severity.Error);
            MudDialog!.Close(DialogResult.Ok(true));
        }

        public void Cancel() => MudDialog!.Cancel();

        public bool Disabled =>
            (CarCreate != null && string.IsNullOrWhiteSpace(CarCreate.Make)) ||
            (CarUpdate != null && string.IsNullOrWhiteSpace(CarUpdate.Make));
    }
}
