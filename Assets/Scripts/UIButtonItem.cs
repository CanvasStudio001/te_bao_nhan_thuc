using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonItem : MonoBehaviour
{
    public Sprite DefaultSprite;
    public Sprite HoverSprite;
    public Sprite SelectSprite;
    public Image BackGround;
    public TextMeshProUGUI text;

    [Header("Color Text")]
    public Color colorTextDefault;
    public Color colorTextSelected;
    public Color colorTextHover;

    [Header("Color Background")]
    public Color colorBackgroundDefault;
    public Color colorBackgroundSelected;
    public Color colorBackgroundHover;
    [SerializeField]
    private bool _isSelected;
    public bool isSelected
    {
        get { return _isSelected; }
        set
        {
            _isSelected = value;
            // Debug.Log("isselected: " + value);
        }
    }


    public void SetDisableButton()
    {
        if (BackGround != null)
        {
            BackGround.color = colorBackgroundDefault;
            if (DefaultSprite != null)
            {
                BackGround.sprite = DefaultSprite;
            }
        }

        if (text != null)
        {
            text.color = colorTextDefault;
            text.fontStyle = FontStyles.Normal;

        }

        isSelected = false;
    }

    public void SetClickButton()
    {

        //if (isSelected)
        //{
        //    SetDisableButton();
        //}
        //else
        {
            if (BackGround != null)
            {
                BackGround.color = colorBackgroundSelected;
                if (SelectSprite != null)
                {
                    BackGround.sprite = SelectSprite;
                }
            }

            if (text != null)
            {
                text.color = colorTextSelected;
                text.fontStyle = FontStyles.Bold;
            }

            isSelected = true;
        }
    }
}
