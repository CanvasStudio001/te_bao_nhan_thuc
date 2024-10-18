using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class ImageZoom : MonoBehaviour
{
    public UIMain _uimain;
    public Button zoomButton;
    public Image _imgBg;
    public Button _btn;
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
        _btn = GetComponentInParent<Button>();
        zoomButton.onClick.AddListener(OnClickedZoom);
        if (_btn != null)
        {
            _btn.onClick.AddListener(OnClickedZoom);
        }

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
