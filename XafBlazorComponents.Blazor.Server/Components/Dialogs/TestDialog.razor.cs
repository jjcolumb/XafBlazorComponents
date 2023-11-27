using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using XafCustomComponents;

namespace XafBlazorComponents.Blazor.Server.Components.Dialogs
{
    public partial class TestDialog : ComponentBase
    {
        [Inject] IDialogService DialogService { get; set; }
        [CascadingParameter] DialogInstance Dialog { get; set; }

        private void OkClickedHandler(MouseEventArgs e)
        {
            //Handle Ok click
            Dialog.Close(new ResponseModel() { Text = "Catch me in Controller", Number = 11 });
        }

        private async void CancelClickedHandler(MouseEventArgs e)
        {
            bool? cancelOperation = await DialogService.ShowMessageBox("Cancel Operation", "Are you sure you want to cancel this operation?", yesText: "Yes", noText: "No");
            if (cancelOperation is not null && (bool)cancelOperation)
                Dialog.Cancel();
        }
    }

    public class ResponseModel
    {
        public string Text { get; set; }
        public int Number { get; set; }
    }
}
