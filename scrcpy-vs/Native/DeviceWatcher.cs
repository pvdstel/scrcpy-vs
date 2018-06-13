using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace scrcpy.VisualStudio.Native
{
    public class DeviceWatcher : IDisposable
    {
        private ManagementEventWatcher _watcher;
        private readonly object _lock = new object();
        private bool _initialized = false;
        private long _lastTrigger;

        public void Initialize()
        {
            lock (_lock)
            {
                if (_initialized) return;

                WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2 OR EventType = 3");
                _watcher = new ManagementEventWatcher(removeQuery);
                _watcher.EventArrived += new EventArrivedEventHandler(OnDeviceChangeEvent);
                _watcher.Start();

                _initialized = true;
            }
        }

        private void OnDeviceChangeEvent(object sender, EventArrivedEventArgs e)
        {
            if (Math.Abs(DateTime.Now.Second - _lastTrigger) > 1)
            {
                DeviceChanged?.Invoke(this, new EventArgs());
            }
            _lastTrigger = DateTime.Now.Second;
        }

        public event EventHandler DeviceChanged;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _watcher.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
