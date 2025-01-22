using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEngine;

public class USBStorageInfo : MonoBehaviour
{
    private string GetUSBStorageDevices()
    {
        var process = new Process();
        process.StartInfo.FileName                  = "wmic";
        process.StartInfo.Arguments                 = "diskdrive where \"MediaType='Removable Media'\" get SerialNumber";
        process.StartInfo.RedirectStandardOutput    = true;
        process.StartInfo.UseShellExecute           = false;
        process.StartInfo.CreateNoWindow            = true;

        process.Start();
        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        return output;
    }

    private string[] ParseUSBStorageInfo(string output)
    {
        try
        {
            var lines       = output.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            var vidRegex    = new Regex(@"VID_([0-9A-F]{4})", RegexOptions.IgnoreCase);
            var pidRegex    = new Regex(@"PID_([0-9A-F]{4})", RegexOptions.IgnoreCase);

            foreach (var line in lines)
            {
                if (line.Contains("USB")) // Chỉ lấy các thiết bị lưu trữ USB
                {
                    var parts = line.Split(new[] { ' ' }, 3, System.StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 3)
                    {
                        var deviceID    = parts[0];
                        var model       = parts[1];
                        var pnpDeviceID = parts[2];

                        var vidMatch    = vidRegex.Match(pnpDeviceID);
                        var pidMatch    = pidRegex.Match(pnpDeviceID);

                        var vendorId    = vidMatch.Success ? vidMatch.Groups[1].Value : "Unknown";
                        var productId   = pidMatch.Success ? pidMatch.Groups[1].Value : "Unknown";

                        UnityEngine.Debug.Log($"Device ID: {deviceID}, Model: {model}, VendorID: {vendorId}, ProductID: {productId}");
                    }
                }
            }
            return lines;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public bool TryGetSerialNumbers(out string[] usbSerialNumbers)
    {
        try
        {
            usbSerialNumbers = GetUSBStorageDevices().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return true;
        }
        catch (Exception)
        {
            usbSerialNumbers = null;
            return false;
        }
    }
}