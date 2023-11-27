using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XafBlazorComponents.Blazor.Server.Components.Dialogs;
using XafBlazorComponents.Module.BusinessObjects;
using XafCustomComponents;

namespace XafBlazorComponents.Blazor.Server.Controllers.TestDialogs
{
    public partial class TestDialogController : ViewController
    {
        #region Services

        IServiceProvider ServiceProvider;
        IDialogService DialogService;
        INotificationService NotificationService;

        #endregion

        #region Controllers



        #endregion

        #region Actions

        SimpleAction ShowTestDialogAction;

        #endregion

        public TestDialogController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Department);

            ShowTestDialogAction = new SimpleAction(this, "ShowTestDialogAction", PredefinedCategory.View)
            {
                Caption = "Show Test Dialog",
            };
            ShowTestDialogAction.Execute += ShowTestDialogAction_Execute;
        }

        #region ViewController Overrided methods

        protected override void OnActivated()
        {
            base.OnActivated();
            ServiceProvider = Application?.ServiceProvider;
            DialogService = ServiceProvider?.GetRequiredService<IDialogService>();
            NotificationService = ServiceProvider.GetRequiredService<INotificationService>();
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        #endregion

        #region Actions EventHandlers

        private async void ShowTestDialogAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IDialogReference dialog = DialogService.Show<TestDialog>("Test Dialog Title");

            DialogResult result = await dialog.Result;
            if (result.Cancelled)
                NotificationService.ShowWarningMessage("Dialog has been cancelled");
            else if (result.Data is not null)
            {
                ResponseModel responseModel = (ResponseModel)result.Data;
                NotificationService.ShowSuccessMessage($"Response model Text: {responseModel.Text} and Number: {responseModel.Number}");
            }
        }

        #endregion

        #region Methods


        #endregion

    }
}
