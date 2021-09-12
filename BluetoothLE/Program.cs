using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;

namespace BluetoothLE
{
    class Program
    {
        static DeviceInformation device = null;

        public static string LED_SERVICE_ID = "ffe0";
        static async Task Main(string[] args)
        {
            // Query for extra properties you want returned
            string[] requestedProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected" };

            DeviceWatcher deviceWatcher =
                        DeviceInformation.CreateWatcher(
                                BluetoothLEDevice.GetDeviceSelectorFromPairingState(false),
                                requestedProperties,
                                DeviceInformationKind.AssociationEndpoint);

            // Register event handlers before starting the watcher.
            // Added, Updated and Removed are required to get all nearby devices
            deviceWatcher.Added += DeviceWatcher_Added;
            deviceWatcher.Updated += DeviceWatcher_Updated;
            deviceWatcher.Removed += DeviceWatcher_Removed;

            // EnumerationCompleted and Stopped are optional to implement.
            deviceWatcher.EnumerationCompleted += DeviceWatcher_EnumerationCompleted;
            deviceWatcher.Stopped += DeviceWatcher_Stopped;

            // Start the watcher.
            deviceWatcher.Start();
            while (true)
            {
                if (device == null)
                {
                    Thread.Sleep(200);
                }
                else
                {
                    Console.WriteLine("Press Any Key to pair with SP110E");
                    Console.ReadKey();
                    BluetoothLEDevice bluetoothLEDevice = await BluetoothLEDevice.FromIdAsync(device.Id);
                    Console.WriteLine("Attempting to pair with SP110E");

                    GattDeviceServicesResult result = await bluetoothLEDevice.GetGattServicesAsync();

                    if(result.Status == GattCommunicationStatus.Success) {
                        Console.WriteLine("Pairing succeeded");
                        var services = result.Services;
                        foreach(var service in services) {
                            if(service.Uuid.ToString("N").Substring(4,4) == LED_SERVICE_ID) {
                                Console.WriteLine("Found LED Service");

                                GattCharacteristicsResult characteristicResult = await service.GetCharacteristicsAsync();
                                if(result.Status == GattCommunicationStatus.Success) {
                                    var characteristics = characteristicResult.Characteristics;
                                    foreach(var characteristic in characteristics) {
                                        Console.WriteLine("------------------");
                                        Console.Write(characteristic+"\n");

                                        GattCharacteristicProperties properties = characteristic.CharacteristicProperties;


                                        if (properties.HasFlag(GattCharacteristicProperties.Notify)) {
                                            
                                            Console.WriteLine("Notify Property found");
                                            GattCommunicationStatus status = await characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                                                GattClientCharacteristicConfigurationDescriptorValue.Notify);
                                            if(status == GattCommunicationStatus.Success) {
                                                characteristic.ValueChanged += Characteristic_ValueChanged;
                                            }
                                        }

                                    }
                                }
                            }
                         }
                    }
                    Console.WriteLine("Press Any Key to Exit application");
                    Console.ReadKey();
                    break;
                }
            }
            deviceWatcher.Stop();
        }

        private static void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args) {
            var reader = DataReader.FromBuffer(args.CharacteristicValue);
            var value = reader.ReadUInt16();
            Console.WriteLine(value);
        }

        private static void DeviceWatcher_Stopped(DeviceWatcher sender, object args)
        {
            //throw new NotImplementedException();
        }

        private static void DeviceWatcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            //throw new NotImplementedException();
        }

        private static void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            //throw new NotImplementedException();
        }

        private static void DeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            //throw new NotImplementedException();
        }

        private static void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
            Console.WriteLine(args.Name);
            if(args.Name == "SP110E")
                device = args;
            //throw new NotImplementedException();
        }
    }
}
