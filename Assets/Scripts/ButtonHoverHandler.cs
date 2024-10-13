using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string description;
    public string Name;
    public void OnPointerEnter(PointerEventData eventData)
    {
   
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        description = "";
        Name = "";
    }
}
