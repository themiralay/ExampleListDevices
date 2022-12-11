using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace ListDevicesTry
{
    class Program
    {
        static void Main(string[] args)
        {
            var usbDevices = GetUSBDevices();

            foreach (var usbDevice in usbDevices)
            {
                Console.WriteLine(" Description: {0}",
                     usbDevice.Description);
            }

            Console.Read();
        }

        static List<USBDeviceInfo> GetUSBDevices()
        {
            string DriveDescription;
            List<USBDeviceInfo> devices = new List<USBDeviceInfo>();
            var devid = "'USB\\VID_04F2&PID_B315\\6&EF94D1A&0&6'";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", @"SELECT * FROM Win32_PnPEntity where DeviceID Like "+devid);
            ManagementObjectCollection collection = searcher.Get();
            if(collection.Count!=0)
            {
                DriveDescription = (string)collection
                    .OfType<ManagementObject>()
                    .FirstOrDefault()
                    .GetPropertyValue("Description");
            }

            foreach (var device in collection)
            {
                devices.Add(new USBDeviceInfo(
                (string)device.GetPropertyValue("DeviceID"),
                (string)device.GetPropertyValue("PNPDeviceID"),
                (string)device.GetPropertyValue("Description")
                ));
            }

            collection.Dispose();
            return devices;
        }
    }

    class USBDeviceInfo
    {
        public USBDeviceInfo(string deviceID, string pnpDeviceID, string description)
        {
            this.DeviceID = deviceID;
            this.PnpDeviceID = pnpDeviceID;
            this.Description = description;
        }
        public string DeviceID { get; private set; }
        public string PnpDeviceID { get; private set; }
        public string Description { get; private set; }
    }
}
