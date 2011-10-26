namespace Vixen.Modules.DisplayPreviewModule.Model
{
    using System;
    using System.Collections.Generic;
    using Vixen.Execution;
    using Vixen.Module.App;
    using Vixen.Modules.DisplayPreviewModule.Views;
    using Vixen.Sys;

    public class DisplayPreviewModuleInstance : AppModuleInstanceBase
    {
        private readonly List<ProgramContext> _programContexts = new List<ProgramContext>();
        private IApplication _application;

        public override IApplication Application
        {
            set
            {
                _application = value;
                InjectAppCommands();
            }
        }

        public override void Dispose()
        {
            EnsureVisualizerIsClosed();
            base.Dispose();
        }

        public override void Loading()
        {
            Execution.ValuesChanged += ExecutionValuesChanged;
            Execution.NodesChanged += ExecutionNodesChanged;
            Execution.ExecutionContextCreated += ExecutionExecutionContextCreated;
            Execution.ExecutionContextReleased += ExecutionExecutionContextReleased;
        }

        public void Setup()
        {
            ViewManager.DisplaySetupView(GetDisplayPreviewModuleDataModel());
        }

        public override void Unloading()
        {
            Execution.NodesChanged -= ExecutionNodesChanged;
            Execution.ValuesChanged -= ExecutionValuesChanged;
            Execution.ExecutionContextCreated -= ExecutionExecutionContextCreated;
            Execution.ExecutionContextReleased -= ExecutionExecutionContextReleased;
        }

        private static void EnsureVisualizerIsClosed()
        {
            ViewManager.EnsureVisualizerIsClosed();
        }

        private static void ExecutionNodesChanged(object sender, EventArgs e)
        {
            // TODO: Remove any channels that are no longer valid.
        }

        private static void ExecutionValuesChanged(ExecutionStateValues stateValues)
        {
            ViewManager.UpdatePreviewExecutionStateValues(stateValues);
        }

        private static void ProgramContextProgramEnded(object sender, EventArgs e)
        {
            Stop();
        }

        private static void Stop()
        {
            EnsureVisualizerIsClosed();
        }

        private void ExecutionExecutionContextCreated(object sender, EventArgs e)
        {
            var programContext = sender as ProgramContext;
            if (programContext != null)
            {
                _programContexts.Add(programContext);
                programContext.ProgramStarted += ProgramContextProgramStarted;
                programContext.ProgramEnded += ProgramContextProgramEnded;
            }
        }

        private void ExecutionExecutionContextReleased(object sender, EventArgs e)
        {
            var programContext = sender as ProgramContext;
            if (programContext != null)
            {
                programContext.ProgramStarted -= ProgramContextProgramStarted;
                programContext.ProgramEnded -= ProgramContextProgramEnded;
                _programContexts.Remove(programContext);
            }
        }

        private DisplayPreviewModuleDataModel GetDisplayPreviewModuleDataModel()
        {
            return (DisplayPreviewModuleDataModel)ModuleData;
        }

        private void InjectAppCommands()
        {
            if (_application == null || _application.AppCommands == null)
            {
                return;
            }

            var appCommand = new AppCommand("Setup Preview Module");
            appCommand.Click += SetupAppCommandClick;
            _application.AppCommands.Add(appCommand);
        }

        private void ProgramContextProgramStarted(object sender, EventArgs e)
        {
            Start();
        }

        private void SetupAppCommandClick(object sender, EventArgs e)
        {
            Setup();
        }

        private void Start()
        {
            ViewManager.StartVisualizer(GetDisplayPreviewModuleDataModel());
        }
    }
}
