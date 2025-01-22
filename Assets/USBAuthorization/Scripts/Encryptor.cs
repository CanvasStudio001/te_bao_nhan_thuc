using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class Encryptor : MonoBehaviour
{
    private string usbs = "usb.bin";
    private const string IV = "1a1a1a1a1a1a1a1a";
    private const string Key = "1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a13";

    public bool TryGetData(int appID, out string[] datas)
    {
        if (appID > 0)
        {
#if UNITY_EDITOR
            var filePath        = $"{Application.streamingAssetsPath}/{usbs}";
#else
            var exeDirectory    = AppDomain.CurrentDomain.BaseDirectory;
            var filePath        = Path.Combine(exeDirectory, usbs);
#endif

            if (File.Exists(filePath))
            {
                using (var stream = File.Open(filePath, FileMode.Open))
                {
                    using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                    {
                        var encrypted = reader.ReadString();
                        var jsonString = Decrypted(encrypted);
                        datas = ParseLesson(jsonString, appID).ToArray();
                        return true;
                    }
                }
            }
        }

        datas = null;
        return false;
    }

    private string Encrypt(string decrypted)
    {
        var textbytes = ASCIIEncoding.ASCII.GetBytes(decrypted);
        var endec = new AesCryptoServiceProvider
        {
            BlockSize = 128,
            KeySize = 256,
            IV = ASCIIEncoding.ASCII.GetBytes(IV),
            Key = ASCIIEncoding.ASCII.GetBytes(Key),
            Padding = PaddingMode.PKCS7,
            Mode = CipherMode.CBC
        };
        var icrypt = endec.CreateEncryptor(endec.Key, endec.IV);
        var enc = icrypt.TransformFinalBlock(textbytes, 0, textbytes.Length);
        icrypt.Dispose();
        return Convert.ToBase64String(enc);
    }

    private string Decrypted(string encrypted)
    {
        var textbytes = Convert.FromBase64String(encrypted);
        var endec = new AesCryptoServiceProvider
        {
            BlockSize = 128,
            KeySize = 256,
            IV = ASCIIEncoding.ASCII.GetBytes(IV),
            Key = ASCIIEncoding.ASCII.GetBytes(Key),
            Padding = PaddingMode.PKCS7,
            Mode = CipherMode.CBC
        };
        var icrypt = endec.CreateDecryptor(endec.Key, endec.IV);
        var enc = icrypt.TransformFinalBlock(textbytes, 0, textbytes.Length);
        icrypt.Dispose();
        return Encoding.ASCII.GetString(enc);
    }

    private List<string> ParseLesson(string jsonString, int id)
    {
        var lessonKey = $"Lesson{id}";
        var jsonData = MiniJSON.Json.Deserialize(jsonString) as Dictionary<string, object> ?? new();

        if (jsonData.TryGetValue(lessonKey, out var rawList))
        {
            var rawJsonList = rawList as string;
            var devices = MiniJSON.Json.Deserialize(rawJsonList) as List<object> ?? new();

            return devices.ConvertAll(d => d.ToString());
        }

        return new();
    }
}