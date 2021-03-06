﻿using scrcpy.VisualStudio.Native;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace scrcpy.VisualStudio.UI
{
    /// <summary>
    /// Interaction logic for WindowHost.xaml
    /// </summary>
    public partial class WindowHost : UserControl, IDisposable
    {
        private Process _process;
        private System.Windows.Forms.Control _host;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowHost"/> class.
        /// </summary>
        public WindowHost()
        {
            InitializeComponent();

            SizeChanged += (s, e) => PositionWindow();
        }

        /// <summary>
        /// Kills the hosted process and any spawned child processes.
        /// </summary>
        public void CleanUp()
        {
            if (_process == null)
            {
                return;
            }

            if (!_process.HasExited)
            {
                ProcessStartInfo killInfo = new ProcessStartInfo()
                {
                    FileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "taskkill.exe"),
                    Arguments = $"/PID {_process.Id} /T /F", // tree and force
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };
                Process kill = Process.Start(killInfo);
                kill.WaitForExit();
            }

            _process = null;
        }

        /// <summary>
        /// Starts the child process using the given information,
        /// and embeds the main window of the started process.
        /// </summary>
        /// <param name="info">The process start information.</param>
        /// <returns>A <see cref="Task"/> instance that completes when the hosted process exits.</returns>
        public async Task<Task> StartProcessAsync(ProcessStartInfo info)
        {
            CleanUp();

            _process = Process.Start(info);

            while (!_process.HasExited && _process.MainWindowHandle == IntPtr.Zero)
            {
                await Task.Delay(1);
            }

            if (!_process.HasExited)
            {
                StealWindow();
            }

            return Task.Run(() => _process.WaitForExit());
        }

        /// <summary>
        /// Steals the window from the desktop and embeds it.
        /// </summary>
        private void StealWindow()
        {
            // Create host surface
            _host = new System.Windows.Forms.Control();
            wfh.Child = _host;
            _host.Focus();

            Methods.SetParent(_process.MainWindowHandle, _host.Handle);
            Methods.SetWindowLongPtr(new HandleRef(null, _process.MainWindowHandle), Constants.GWL_STYLE, new IntPtr(Constants.WS_VISIBLE));

            PositionWindow();
        }

        /// <summary>
        /// Positions the hosted window appropriately.
        /// </summary>
        private void PositionWindow()
        {
            if (_process == null || _process.HasExited)
            {
                return;
            }

            if (_host == null)
            {
                return;
            }

            Methods.MoveWindow(_process.MainWindowHandle, 0, 0, (int)ActualWidth, (int)ActualHeight, true);
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
