using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class USBUnlock : MonoBehaviour
{
    public int appID = 0;
    public GameObject unlock;
    private Encryptor encryptor;
    private USBStorageInfo usbStorageInfo;

    void Start()
    {
        if (appID > 0)
        {
            encryptor = GetComponent<Encryptor>();
            usbStorageInfo = GetComponent<USBStorageInfo>();
            StartCoroutine(CheckUnlockRoutine());
        }
    }

    private IEnumerator CheckUnlockRoutine()
    {
        while (true)
        {
            unlock.SetActive(!CheckUnlock());
            yield return new WaitForSeconds(1f);
        }
    }

    private bool CheckUnlock()
    {
        if (encryptor.TryGetData(appID, out var datas))
        {
            if (usbStorageInfo.TryGetSerialNumbers(out var infos))
            {
                foreach (var data in datas)
                {
                    foreach (var info in infos)
                    {
                        if (data.Contains(info.Trim()))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnClickResize()
    {
        var originalWidth = Screen.currentResolution.width;
        var originalHeight = Screen.currentResolution.height;

        if (Screen.width == originalWidth && Screen.height == originalHeight)
        {
            Screen.SetResolution(originalWidth / 2, originalHeight / 2, false);
        }
        else
        {
            Screen.SetResolution(originalWidth, originalHeight, true);
        }
    }
}
