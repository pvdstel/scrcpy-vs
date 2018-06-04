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
    public partial class ScrcpyToolWindowControl : UserControl, IDisposable
    {
        private SemaphoreSlim _pageSemaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrcpyToolWindowControl"/> class.
        /// </summary>
        public ScrcpyToolWindowControl()
        {
            this.InitializeComponent();

            ScrcpyViewModel vm = new ScrcpyViewModel();
            vm.ScrcpyStartRequested += ScrcpyStartRequested;
            vm.ScrcpyStopRequested += (s, e) => windowHost.CleanUp();
            DataContext = vm;
        }

        private async void ScrcpyStartRequested(object sender, Model.ScrcpyEventArgs e)
        {
            scrcpyStarting.Visibility = Visibility.Visible;
            Task processCompletionTask = await windowHost.StartProcess(e.ProcessStartInfo);

            await _pageSemaphore.WaitAsync();
            pageControl.SelectedIndex = 1;
            scrcpyStarting.Visibility = Visibility.Collapsed;

            await processCompletionTask;

            pageControl.SelectedIndex = 0;
            _pageSemaphore.Release();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
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