using Services.Common.Dto;

namespace ViewModels
{
    public class OwnerFormViewModel : ComponentBaseViewModel
    {
        [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
        protected const string ValidationMessage = "Field is required.";

        [Parameter]
        public OwnerCreateDto? OwnerCreate { get; set; }

        [Parameter]
        public OwnerUpdateDto? OwnerUpdate { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (OwnerUpdate != null)
            {
                StateHasChanged();
            }
        }

        public async Task CreateOrUpdate()
        {
            try
            {
                var response = new GeneralResponseDto();
                if (OwnerCreate != null && OwnerCreate.OwnerId == 0)
                {
                    response = await OwnerService!.Create(OwnerCreate);
                }
                else
                {
                    response = await OwnerService!.Update(OwnerUpdate!.OwnerId, OwnerUpdate);
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
            (OwnerCreate != null && string.IsNullOrWhiteSpace(OwnerCreate.FirstName)) ||
            (OwnerUpdate != null && string.IsNullOrWhiteSpace(OwnerUpdate.FirstName));
    }
}
