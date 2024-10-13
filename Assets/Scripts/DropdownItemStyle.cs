using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropdownItemStyle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TextMeshProUGUI label;
    public Image background;
    public Image backgroundHover;

    public Color defaultBackgroundColor = Color.black;
    public Color defaultTextColor = Color.white;
    public Color hoverBackgroundColor = Color.white;
    public Color hoverTextColor = Color.black;

    private void Awake()
    {

        // Set default colors
        if (background != null)
        {
            background.color = defaultBackgroundColor;
        }

        if (label != null)
        {
            label.color = defaultTextColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        SetHoverColors();
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        SetDefaultColors();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        backgroundHover.gameObject.SetActive(false);
    }

    private void SetHoverColors()
    {


        if (label != null)
        {
            //if (GetComponent<UIButtonItem>().isSelected)
            //{
            //    label.color = hoverTextColor;
            //    backgroundHover.gameObject.SetActive(false);
            //}
            //else
            if (!GetComponent<UIButtonItem>().isSelected)
            {
                // label.color = hoverBackgroundColor;
                backgroundHover.gameObject.SetActive(true);
                backgroundHover.color = hoverBackgroundColor;
            }
        }
    }

    private void SetDefaultColors()
    {


        if (label != null)
        {
            //if (GetComponent<UIButtonItem>().isSelected)
            //{
            //    label.color = hoverTextColor;
            //    backgroundHover.gameObject.SetActive(false);
            //}
            //else

            {
                // label.color = defaultTextColor;
                backgroundHover.gameObject.SetActive(false);
                backgroundHover.color = defaultBackgroundColor;
            }
        }
    }
}
