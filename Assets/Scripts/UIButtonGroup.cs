using UnityEngine;
using UnityEngine.UI;

public class UIButtonGroup : MonoBehaviour
{
    [SerializeField]
    private Button[] _groupButton;

    [SerializeField]
    private int defaultIndex = 0;

    private void Awake()
    {
        foreach (var button in _groupButton)
        {
            //button.onClick.AddListener(() => { ButtonSelectGroup(button); });
        }
    }

    public void ButtonSelectGroup(Button uIButton)
    {
        foreach (var button in _groupButton)
        {
            var buttonItem = button.GetComponent<UIButtonItem>();
            if (buttonItem != null)
            {
                if (button == uIButton)
                {
                    buttonItem.SetClickButton();
                }
                else
                {
                    buttonItem.SetDisableButton();
                }
            }
        }
    }
}
