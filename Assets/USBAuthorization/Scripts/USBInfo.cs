using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEngine;

public class USBInfo : MonoBehaviour
{
    void Start()
    {
        GetUSBDevices();
    }

    void GetUSBDevices()
    {
        var process = new Process();
        process.StartInfo.FileName = "wmic";
        process.StartInfo.Arguments = "diskdrive where \"MediaType='Removable Media'\" get DeviceID, Model, PNPDeviceID, Name";
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        process.Start();
        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        ParseUSBInfo(output);
    }

    void ParseUSBInfo(string output)
    {
        var lines = output.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        var vidRegex = new Regex(@"VID_([0-9A-F]{4})", RegexOptions.IgnoreCase);
        var pidRegex = new Regex(@"PID_([0-9A-F]{4})", RegexOptions.IgnoreCase);

        foreach (var line in lines)
        {
            if (line.Contains("USB\\") || line.Contains("USB")) // Chỉ lấy các thiết bị USB
            {
                var parts = line.Split(new[] { ' ' },4, System.StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 4)
                {

                    var deviceID = parts[0];
                    var model = parts[1];
                    var pnpDeviceID = parts[2];
                    var name = parts[3];

                    var vidMatch = vidRegex.Match(line);
                    var pidMatch = pidRegex.Match(line);

                    var vendorId = vidMatch.Success ? vidMatch.Groups[1].Value : "Unknown";
                    var productId = pidMatch.Success ? pidMatch.Groups[1].Value : "Unknown";

                    UnityEngine.Debug.Log($"Device Name: {name}, VendorID: {vendorId}, ProductID: {productId}");
                }
            }
        }
    }
}
