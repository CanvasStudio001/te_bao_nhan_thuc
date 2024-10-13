using UnityEngine;
using UnityEngine.UI;

public class ImageZoom : MonoBehaviour
{
    public UIMain _uimain;
    public Button zoomButton;
    public Image _imgBg;
    private void Awake()
    {
        if (_imgBg == null)
        {
            _imgBg = GetComponentInParent<Image>();
        }
        if (_uimain == null)
        {
            _uimain = GetComponentInParent<UIMain>();
        }
        zoomButton.onClick.AddListener(OnClickedZoom);
    }
    private void OnClickedZoom()
    {
        if (_uimain != null)
        {
            _uimain.SetActiveBGPanel(_imgBg.sprite);
        }
        else
        {
            Debug.LogError("_uimain null");
        }
    }

}
