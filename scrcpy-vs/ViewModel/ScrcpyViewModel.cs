using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using scrcpy.VisualStudio.Android;
using scrcpy.VisualStudio.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace scrcpy.VisualStudio.ViewModel
{
    /// <summary>
    /// A viewmodel for the scrcpy tool window.
    /// </summary>
    public class ScrcpyViewModel : ViewModelBase
    {
        private ObservableCollection<Device> _devices;
        private Device _selectedDevice;
        private bool _isGettingDevices;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ScrcpyViewModel()
        {
            GetDevices();
            StartScrcpyCommand = new RelayCommand<Device>(StartScrcpy);
            StopScrcpyCommand = new RelayCommand(StopScrcpy);
            RefreshDevicesCommand = new RelayCommand(GetDevices);
        }

        /// <summary>
        /// Gets or sets the devices.
        /// </summary>
        public ObservableCollection<Device> Devices
        {
            get => _devices;
            set
            {
                _devices = value;
                RaisePropertyChanged(nameof(Devices));
            }
        }

        /// <summary>
        /// Gets or sets the selected devices.
        /// </summary>
        public Device SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                _selectedDevice = value;
                RaisePropertyChanged(nameof(SelectedDevice));
            }
        }

        /// <summary>
        /// Gets whether the list of devices is being refreshed.
        /// </summary>
        public bool IsGettingDevices
        {
            get => _isGettingDevices;
            private set
            {
                _isGettingDevices = value;
                RaisePropertyChanged(nameof(IsGettingDevices));
            }
        }

        /// <summary>
        /// Gets the command used to call <see cref="StartScrcpy(Device)"/>.
        /// </summary>
        public RelayCommand<Device> StartScrcpyCommand { get; }

        /// <summary>
        /// Gets the command used to call <see cref="StartScrcpy(Device)"/>.
        /// </summary>
        public RelayCommand StopScrcpyCommand { get; }

        /// <summary>
        /// Gets the command used to call <see cref="GetDevices"/>.
        /// </summary>
        public RelayCommand RefreshDevicesCommand { get; }

        /// <summary>
        /// Gets the connected Android devices.
        /// </summary>
        public async void GetDevices()
        {
            IsGettingDevices = true;
            var devicesTasks = (await AdbWrapper.GetAuthorizedDevicesAsync())
                .Select(async s => new Device(s, $"{await AdbWrapper.GetDeviceManufacturerAsync(s)} {await AdbWrapper.GetDeviceModelAsync(s)}"))
                .ToList();
            await Task.WhenAll(devicesTasks);

            Devices = new ObservableCollection<Device>(devicesTasks.Select(t => t.Result));
            if (Devices.Count > 0) SelectedDevice = Devices.First();
            IsGettingDevices = false;
        }

        /// <summary>
        /// Sends the UI a message to start scrcpy.
        /// </summary>
        /// <param name="device">The device to connect to.</param>
        public void StartScrcpy(Device device)
        {
            ScrcpyStartRequested?.Invoke(this, new ScrcpyEventArgs(ScrcpyWrapper.GetStartInfo(device.ID)));
        }

        /// <summary>
        /// Sends the UI a message to stop scrcpy.
        /// </summary>
        /// <param name="device">The device to connect to.</param>
        public void StopScrcpy()
        {
            ScrcpyStopRequested?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Invoked when scrcpy is requested to be started.
        /// </summary>
        public event EventHandler<ScrcpyEventArgs> ScrcpyStartRequested;

        /// <summary>
        /// Invoked when scrcpy is requested to be stopped.
        /// </summary>
        public event EventHandler ScrcpyStopRequested;
    }
}