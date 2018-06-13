namespace scrcpy.VisualStudio.UI
{
    using Microsoft.VisualStudio.Threading;
    using scrcpy.VisualStudio.Android;
    using scrcpy.VisualStudio.Native;
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
    public partial class ScrcpyToolWindowControl : UserControl, IDisposable
    {
        /// <summary>
        /// The delay introduced before refreshing devices.
        /// </summary>
        private const int DeviceChangeDelay = 1000;

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
        /// An instance of <see cref="DeviceWatcher"/>.
        /// </summary>
        private DeviceWatcher _deviceWatcher;

        /// <summary>
        /// The joinable task factory.
        /// </summary>
        JoinableTaskFactory jtf;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrcpyToolWindowControl"/> class.
        /// </summary>
        public ScrcpyToolWindowControl()
        {
            _viewModel = new ScrcpyViewModel();
            _viewModel.ScrcpyStartRequested += ScrcpyStartRequested;
            _viewModel.ScrcpyStopRequested += (s, e) => windowHost.CleanUp();
            DataContext = _viewModel;

            jtf = new JoinableTaskFactory(new JoinableTaskContext());
            _deviceWatcher = new DeviceWatcher();
            async void deviceChangedHandler(object s, EventArgs e) => await DevicesChangedEventAsync();
            _deviceWatcher.DeviceChanged += deviceChangedHandler;
            _deviceWatcher.Initialize();

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

            Task processCompletionTask = await windowHost.StartProcessAsync(e.ProcessStartInfo);
            await _pageSemaphore.WaitAsync();

            pageControl.SelectedIndex = 1;
            _viewModel.IsStartingScrcpy = false;

            await processCompletionTask;

            pageControl.SelectedIndex = 0;
            _pageSemaphore.Release();
        }

        /// <summary>
        /// Invoked when the device configuration changes.
        /// </summary>
        /// <returns></returns>
        private async Task DevicesChangedEventAsync()
        {
            await jtf.SwitchToMainThreadAsync();
            await Task.Delay(DeviceChangeDelay);
            await _viewModel.GetDevicesAsync();
        }

        private async void pageControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_firstTimeLoad)
            {
                _firstTimeLoad = false;
                await _viewModel.GetDevicesAsync();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _deviceWatcher.Dispose();
                }

                AdbWrapper.KillServer();

                disposedValue = true;
            }
        }

        ~ScrcpyToolWindowControl()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}