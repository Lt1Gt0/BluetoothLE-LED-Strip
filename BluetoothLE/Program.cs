using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.Generic;
using System.Diagnostics;

namespace BluetoothLE
{
    class Program
    {
        static DeviceInformation device = null;

        public static string LED_SERVICE_ID = "ffe0";
        public static GattCharacteristic ffe1 = null;
        public static GattCharacteristic ffe2 = null;
        public static List<LedStrip> ledStrips = new List<LedStrip>();

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
            while (true) {
                if (device == null) {
                    Thread.Sleep(200);
                }
                else {
                    Console.WriteLine("Press Any Key to pair with SP110E");
                    Console.ReadKey();
                    BluetoothLEDevice bluetoothLEDevice = await BluetoothLEDevice.FromIdAsync(device.Id);
                    Console.WriteLine("Attempting to pair with SP110E");

                    GattDeviceServicesResult result = await bluetoothLEDevice.GetGattServicesAsync();

                    if (result.Status == GattCommunicationStatus.Success) {
                        Console.WriteLine("Pairing succeeded");
                        var services = result.Services;
                        foreach (var service in services) {
                            if (service.Uuid.ToString("N").Substring(4, 4) == LED_SERVICE_ID) {
                                Console.WriteLine("Found LED Service");

                                GattCharacteristicsResult characteristicResult = await service.GetCharacteristicsAsync();
                                if (result.Status == GattCommunicationStatus.Success) {
                                    LedStrip ledStrip = new LedStrip(service);
                                    ledStrips.Add(ledStrip);
                                    await ledStrips[0].setRGB(0xFF, 0xFF, 0xFF);
                                }
                            }
                        }
                    }
                    while (true) {
                        Thread.Sleep(1000);
                        try {
                            Process amogus = Process.GetProcessesByName("Among Us")[0];
                            if(amogus == null) {
                                continue;
                            }
                            foreach(LedStrip led in ledStrips) {
                                await led.susMode();
                            }
                            amogus.WaitForExit();
                            foreach (LedStrip led in ledStrips) {
                                await led.setRGB(0xFF, 0xFF, 0xFF);
                            }
                        }
                        catch{
                            continue;
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
