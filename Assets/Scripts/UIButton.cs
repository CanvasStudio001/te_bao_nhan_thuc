using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    public TextMeshProUGUI numberButton;
    public TextMeshProUGUI nameButton;
    public Image imgCicle;
    public Image imgBackgroud;
    public Sprite spriteCicleDefault;
    public Sprite spriteBackgroudDefault;
    public Sprite spriteCicleSelected;
    public Sprite spriteBackgroudSelected;
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    public void SetBackground(bool isActive)
    {
        if (isActive)
        {
            imgCicle.sprite = spriteCicleSelected;
            imgBackgroud.sprite = spriteBackgroudSelected;
            numberButton.color = Color.black;
            nameButton.color = Color.black;

            //button.interactable = false;
        }
        else
        {
            imgCicle.sprite = spriteCicleDefault;
            imgBackgroud.sprite = spriteBackgroudDefault;
            numberButton.color = Color.white;
            nameButton.color = Color.white;

            // button.interactable = true;
        }
        //Debug.Log("numberButton: " + nameButton.text + " isActive: " + isActive);
    }

}
