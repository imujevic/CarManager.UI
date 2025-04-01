using Services.Common.Dto;

namespace ViewModels
{
    public class InspectionFormViewModel : ComponentBaseViewModel
    {
        [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
        protected const string ValidationMessage = "Field is required.";

        [Parameter]
        public InspectionCreateDto? InspectionCreate { get; set; }

        [Parameter]
        public InspectionUpdateDto? InspectionUpdate { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (InspectionUpdate != null)
            {
                StateHasChanged();
            }
        }

        public async Task CreateOrUpdate()
        {
            try
            {
                var response = new GeneralResponseDto();
                if (InspectionCreate != null && InspectionCreate.InspectionId == 0)
                {
                    response = await InspectionService!.Create(InspectionCreate);
                }
                else
                {
                    response = await InspectionService!.Update(InspectionUpdate!.InspectionId, InspectionUpdate);
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
            (InspectionCreate != null && string.IsNullOrWhiteSpace(InspectionCreate.Status)) ||
            (InspectionUpdate != null && string.IsNullOrWhiteSpace(InspectionUpdate.Status));
    }
}
