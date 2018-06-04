using scrcpy.VisualStudio.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace scrcpy.VisualStudio.UI
{
    /// <summary>
    /// Interaction logic for WindowHost.xaml
    /// </summary>
    public partial class WindowHost : UserControl, IDisposable
    {
        private Process _process;
        private System.Windows.Forms.Control _host;

        public WindowHost()
        {
            InitializeComponent();

            SizeChanged += (s, e) => PositionWindow();
        }

        public void CleanUp()
        {
            if (_process == null) return;

            if (!_process.HasExited) _process.Kill();
            _process = null;
        }

        public async Task<Task> StartProcess(ProcessStartInfo info)
        {
            CleanUp();

            _process = Process.Start(info);
            await Task.Run(() => _process.WaitForInputIdle());

            StealWindow();

            return Task.Run(() => _process.WaitForExit());
        }

        private void StealWindow()
        {
            _host = new System.Windows.Forms.Control();
            wfh.Child = _host;
            _host.Focus();

            Methods.SetParent(_process.MainWindowHandle, _host.Handle);
            Methods.SetWindowLongPtr(new HandleRef(null, _process.MainWindowHandle), Constants.GWL_STYLE, new IntPtr(Constants.WS_VISIBLE));

            PositionWindow();
            wfh.Focus();
        }

        private void PositionWindow()
        {
            if (_process == null || _process.HasExited) return;
            if (_host == null) return;

            Methods.MoveWindow(_process.MainWindowHandle, 0, 0, (int)ActualWidth, (int)ActualHeight, true);
        }

        public event EventHandler ProcessExited;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                CleanUp();
                disposedValue = true;
            }
        }

        ~WindowHost()
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
