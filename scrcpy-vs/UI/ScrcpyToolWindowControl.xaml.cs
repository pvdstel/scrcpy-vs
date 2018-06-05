namespace scrcpy.VisualStudio.UI
{
    using scrcpy.VisualStudio.Android;
    using scrcpy.VisualStudio.ViewModel;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ScrcpyToolWindowControl.
    /// </summary>
    public partial class ScrcpyToolWindowControl : UserControl
    {
        /// <summary>
        /// A semaphore used to ensure that race conditions do not occur in the UI.
        /// </summary>
        private SemaphoreSlim _pageSemaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// The viewmodel.
        /// </summary>
        private ScrcpyViewModel _viewModel;

        private bool _firstTimeLoad = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrcpyToolWindowControl"/> class.
        /// </summary>
        public ScrcpyToolWindowControl()
        {
            _viewModel = new ScrcpyViewModel();
            _viewModel.ScrcpyStartRequested += ScrcpyStartRequested;
            _viewModel.ScrcpyStopRequested += (s, e) => windowHost.CleanUp();
            DataContext = _viewModel;

            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when scrcpy is requested to be started.
        /// </summary>
        /// <param name="sender">The event generator.</param>
        /// <param name="e">The event arguments.</param>
        private async void ScrcpyStartRequested(object sender, Model.ScrcpyEventArgs e)
        {
            _viewModel.IsStartingScrcpy = true;
            Task processCompletionTask = await windowHost.StartProcess(e.ProcessStartInfo);

            await _pageSemaphore.WaitAsync();
            pageControl.SelectedIndex = 1;
            _viewModel.IsStartingScrcpy = false;

            await processCompletionTask;

            pageControl.SelectedIndex = 0;
            _pageSemaphore.Release();
        }

        private async void pageControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_firstTimeLoad)
            {
                _firstTimeLoad = false;
                await _viewModel.GetDevicesAsync();
            }
        }
    }
}