using TMPro;
using UnityEngine;

public class ScrollViewItem : MonoBehaviour
{
    public TextMeshProUGUI _txtContent;
    public void SetContent(string content)
    {
        _txtContent.text = content;
    }
}