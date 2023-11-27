using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel;
using XafCustomComponents;

namespace XafBlazorComponents.Blazor.Server.Components.ProgressIndicators
{
    public partial class ProgressIndicatorDialog : ComponentBase
    {
        [Inject] IDialogService DialogService { get; set; }
        [CascadingParameter] DialogInstance Dialog { get; set; }
        [Parameter] public BackgroundWorker Worker { get; set; }

        public ProgressIndicatorState State { get; set; }
        int Percentage { get; set; }
        public string OperationElementsDetail { get; set; }
        public string OperationCurrentElementDetail { get; set; }
        public bool Indeterminate { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Dialog.CloseOnEscapeKey = false;
            Dialog.CloseOnOutsideClick = false;
            Dialog.CloseButton = false;
        }

        private void ProgressChangedHandler(object state)
        {
            State = state as ProgressIndicatorState;
            Percentage = State.CurrentOperationPercentage;
            OperationElementsDetail = State.OperationElementsDetail;
            OperationCurrentElementDetail = State.OperationCurrentElementDetail;
            Indeterminate = State.Indeterminate;
        }

        private async void CancelClickedHandler(MouseEventArgs e)
        {
            bool? cancelOperation = await DialogService.ShowMessageBox("Cancel Operation", "Are you sure you want to cancel this operation?", yesText: "Yes", noText: "No");
            if (cancelOperation is not null && (bool)cancelOperation)
                Dialog?.Cancel();
        }

        private void OkClickedHandler(MouseEventArgs e)
        {
            Dialog.Close();
        }
    }
}
