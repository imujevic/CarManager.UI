using Services.Common.Dto;

namespace ViewModels
{
    public class ServiceCentarFormViewModel : ComponentBaseViewModel
    {
        [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
        protected const string ValidationMessage = "Field is required.";

        [Parameter]
        public ServiceCentarCreateDto? ServiceCentarCreate { get; set; }

        [Parameter]
        public ServiceCentarUpdateDto? ServiceCentarUpdate { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (ServiceCentarUpdate != null)
            {
                StateHasChanged();
            }
        }

        public async Task CreateOrUpdate()
        {
            try
            {
                var response = new GeneralResponseDto();
                if (ServiceCentarCreate != null && ServiceCentarCreate.ServiceCentarId == 0)
                {
                    response = await ServiceCentarService!.Create(ServiceCentarCreate);
                }
                else
                {
                    response = await ServiceCentarService!.Update(ServiceCentarUpdate!.ServiceCentarId, ServiceCentarUpdate);
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
            (ServiceCentarCreate != null && string.IsNullOrWhiteSpace(ServiceCentarCreate.Status)) ||
            (ServiceCentarUpdate != null && string.IsNullOrWhiteSpace(ServiceCentarUpdate.Status));
    }
}
