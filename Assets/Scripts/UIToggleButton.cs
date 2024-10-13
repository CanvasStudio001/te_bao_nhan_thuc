using UnityEngine;
using UnityEngine.UI;

public class UIToggleButton : MonoBehaviour
{
    public Sprite OnSprite;
    public Sprite OffSprite;
    public Image BackGround;
    private bool _isOn;
    public bool isOn
    {
        get { return _isOn; }
        set { _isOn = value; UpdateSprite(); }
    }
    private void Awake()
    {
        BackGround = GetComponent<Image>();
        UpdateSprite();
        this.GetComponent<Button>().onClick.AddListener(Toggle);
    }

    public void Toggle()
    {
        _isOn = !_isOn;
        UpdateSprite();
    }
    private void UpdateSprite()
    {
        if (BackGround != null)
        {
            BackGround.sprite = _isOn ? OnSprite : OffSprite; // Thay đổi sprite dựa trên trạng thái
        }
    }
}
