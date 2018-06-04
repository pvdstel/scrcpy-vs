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

        private RelayCommand<Device> _startScrcpyCommand;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ScrcpyViewModel()
        {
            GetDevices();
            _startScrcpyCommand = new RelayCommand<Device>(StartScrcpy);
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
        /// Gets the command used to call <see cref="StartScrcpy(Device)"/>.
        /// </summary>
        public RelayCommand<Device> StartScrcpyCommand => _startScrcpyCommand;

        /// <summary>
        /// Gets the connected Android devices.
        /// </summary>
        public async void GetDevices()
        {
            var devicesTasks = (await Adb.GetAuthorizedDevices())
                .Select(async s => new Device(s, $"{await Adb.GetDeviceManufacturer(s)} {await Adb.GetDeviceModel(s)}"))
                .ToList();
            await Task.WhenAll(devicesTasks);

            Devices = new ObservableCollection<Device>(devicesTasks.Select(t => t.Result));
            if (Devices.Count > 0) SelectedDevice = Devices.First();
        }

        /// <summary>
        /// Sends the UI a message to start scrcpy.
        /// </summary>
        /// <param name="device">The device to connect to.</param>
        public void StartScrcpy(Device device)
        {
            ScrcpyStartRequested?.Invoke(this, new ScrcpyEventArgs(device));
        }

        public event EventHandler<ScrcpyEventArgs> ScrcpyStartRequested;
        public event EventHandler<ScrcpyEventArgs> ScrcpyStopRequested;
    }
}