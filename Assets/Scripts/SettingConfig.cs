using UnityEngine;

public class SettingConfig : MonoBehaviour
{
    private static SettingConfig instance;
    public static SettingConfig Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SettingConfig>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(SettingConfig).Name;
                    instance = obj.AddComponent<SettingConfig>();
                }
            }
            return instance;
        }
    }

    public bool isInfoPanelOn = false;
    public bool isTutorialPanelOn = false;
    public bool isAutoPlayOn = false;
    public bool isTurnOnUI = false;
    public bool isFullScreen = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
