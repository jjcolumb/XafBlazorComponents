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
using System.ComponentModel;
using System.Linq;
using System.Text;
using XafBlazorComponents.Blazor.Server.Components.ProgressIndicators;
using XafBlazorComponents.Module.Enums;
using XafBlazorComponents.Module.Helpers.ProgressIndicator;
using XafCustomComponents;

namespace XafBlazorComponents.Blazor.Server.Controllers.TestProgressIndicators
{
    public partial class TestProgressIndicatorController : ViewController
    {
        #region Services

        IServiceProvider ServiceProvider;
        IDialogService DialogService;
        INotificationService NotificationService;

        #endregion

        #region Controllers



        #endregion

        #region Actions

        SimpleAction ShowProgressIndicatorAction;

        #endregion

        BackgroundWorker TestWorker;
        WorkerStatus TestWorkerStatus;

        public TestProgressIndicatorController()
        {
            InitializeComponent();

            ShowProgressIndicatorAction = new SimpleAction(this, "ShowProgressIndicatorAction", PredefinedCategory.View)
            {
                Caption = "Show Progress Indicator",
            };

            ShowProgressIndicatorAction.Execute += ShowProgressIndicatorAction_Execute;
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
            ReleaseWorkers();
            base.OnDeactivated();
        }

        #endregion

        #region Actions EventHandlers

        private async void ShowProgressIndicatorAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            InitializeTestWorker();
            TestWorker.RunWorkerAsync();

            DialogParameters parameters = new DialogParameters() { { "Worker", TestWorker } };
            IDialogReference dialog = DialogService.Show<ProgressIndicatorDialog>("Background Action Excecution", parameters);

            var result = await dialog.Result;
            if (result.Cancelled)
                TestWorkerStatus = WorkerStatus.Cancelled;
        }

        #endregion

        #region Methods

        private void InitializeTestWorker()
        {
            TestWorker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            TestWorker.DoWork += TestWorker_DoWork;
            TestWorker.RunWorkerCompleted += TestWorker_RunWorkerCompleted;
            TestWorkerStatus = WorkerStatus.Running;
        }

        private void TestWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (TestWorkerStatus != WorkerStatus.Running)
            {
                e.Cancel = true;
                return;
            }
            int percentage = 0;
            TestWorker.ReportProgress(percentage);
            ProgressIndicatorState state = new ProgressIndicatorState();
            for (int i = 1; i <= 10; i++)
            {
                if (TestWorkerStatus != WorkerStatus.Running)
                {
                    e.Cancel = true;
                    return;
                }
                percentage = ProgressIndicatorHelper.GetIntegerPercentage(i, 10);
                state.CurrentOperationPercentage = percentage;
                state.OperationElementsDetail = $"Calculating: item {i} of {10}";
                state.OperationCurrentElementDetail = $"Name of item {i}";
                TestWorker.ReportProgress(percentage, state);
                System.Threading.Thread.Sleep(500);
            }
            state.Indeterminate = true;
            TestWorker.ReportProgress(percentage, state);
            System.Threading.Thread.Sleep(2000);
        }

        private void TestWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error is not null)
            {
                //TODO: Show exception message
            }
            else if (e.Cancelled)
            {
                View.ObjectSpace.Rollback(false);
            }
            else
            {
                if (View.ObjectSpace.IsModified)
                {
                    //View.ObjectSpace.CommitChanges();
                }
            }
            TestWorkerStatus = WorkerStatus.Stopped;
            TestWorker?.Dispose();
        }

        private void ReleaseWorkers()
        {
            TestWorkerStatus = WorkerStatus.Stopped;
            TestWorker?.Dispose();
        }

        #endregion
    }
}
