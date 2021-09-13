using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Phone.System.UserProfile.GameServices.Core;

public class LedStrip {
    private GattDeviceService Service;
    private GattCharacteristic IO;
    private const ushort characteristicID = 0xffe1;
    public LedStrip(GattDeviceService service) {
        Service = service;
        getCharacteristics();
        initializeIO();
    }

    private async void getCharacteristics() {
        foreach (var characteristic in (await Service.GetCharacteristicsAsync()).Characteristics) {

            ushort UUID = BitConverter.ToUInt16(characteristic.Uuid.ToByteArray(), 0);
            switch (UUID) {
                case characteristicID:
                IO = characteristic;
                break;
                case 0xffe2:
                initializeFFE2(characteristic);
                break;
            }
        }
    }

    private async void initializeIO(){
        byte[] ffe1Buffer = new byte[] { 0x01, 0xb7, 0xe3, 0xd5 };
        var ffe1Result = await IO.WriteValueAsync(ffe1Buffer.AsBuffer());
        if(ffe1Result != GattCommunicationStatus.Success) {
            throw new Exception("Failed to communicate with SP110E");
        }
    }
    private async void initializeFFE2(GattCharacteristic ffe2){
        byte[] ffe1Buffer = new byte[] { 0x01, 0x00};
        var ffe1Result = await ffe2.WriteValueAsync(ffe1Buffer.AsBuffer());
        if (ffe1Result != GattCommunicationStatus.Success) {
            throw new Exception("Failed to communicate with SP110E");
        }
    }

    public async Task<GattCommunicationStatus> turnOn() {
        byte[] byteArrayBuffer = new byte[] { 0xfa, 0x0e, 0xc7, 0xaa };
        return await IO.WriteValueAsync(byteArrayBuffer.AsBuffer());
    }
    public async Task<GattCommunicationStatus> turnOff() {
        byte[] byteArrayBuffer = new byte[] { 0xb0, 0x4f, 0xc2, 0xab };
        return await IO.WriteValueAsync(byteArrayBuffer.AsBuffer());
    }
    public async Task<GattCommunicationStatus> setRGB(byte red, byte green, byte blue) {
        byte[] byteArrayBuffer = new byte[] { red, green, blue, 0x1e };
        return await IO.WriteValueAsync(byteArrayBuffer.AsBuffer());
    }
    public async Task<GattCommunicationStatus> setPreset(byte preset) {
        byte[] presetBuffer = new byte[] { preset, 0x00, 0x00, 0x2c };
        return await IO.WriteValueAsync(presetBuffer.AsBuffer());
    }
    public async Task<GattCommunicationStatus> susMode() {    
        for(int i = 0; i < 255; i+=15) {
            await setRGB((byte)i, 0x00, 0x00);
            Thread.Sleep(40);
        }

        return await setPreset(5);
    }

}