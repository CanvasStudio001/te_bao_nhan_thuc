using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIMain : MonoBehaviour
{
    [SerializeField] TestScroll _testScrollTV;
    [SerializeField] TestScroll _testScrollDV;
    TestScroll _testScroll;

    [Header("Button")]
    public Button[] totalButtons;
    public ScrollRect ScrollRect;
    public ScrollRect ScrollRectDV;


    public Button[] groupButtonLeft;
    [SerializeField]
    private List<string> nameButtonList;
    [SerializeField]
    private List<string> nameButtonListDV;

    public Button NextButton;
    public Button PreviousButton;
    public Image ImgNextButton;
    public Image ImgPreviousButton;
    public TextMeshProUGUI nameButtonCurrent;
    private int currentSelected;
    public int CurrentButtonSelected
    {
        get { return currentSelected; }
        set
        {
            currentSelected = value;
            Debug.Log("currentSelected: " + currentSelected);
        }
    }
    public Button AutoPlayBtn;
    public Button TurnUIBtn;
    public Button InforBtn;
    public Button TutorialBtn;
    public Button ResetBtn;
    public Button FullScreenBtn;
    public Button ExitBtn;
    public Button CutModelBtn;
    public Button ShowDropdownBtn;
    public bool IsClickSwipe;

    [Header("Panel")]
    //public GameObject modelHalf;
    //public GameObject modelFull;
    public List<GameObject> ListParticirle;
    public GameObject uiInforPanel;
    public GameObject TutorialPanel;
    public GameObject DescriptionPanel;
    public GameObject Dropdown;
    public GameObject Swipe;
    [SerializeField]
    private GameObject swipeDV;
    public CinemachineFreeLook freeLookCamera;
    public FreeCameraController controller;
    public CameraControl cameraControl;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI DescriptionText;
    [SerializeField]
    private List<GameObject> Descriptions;
    [SerializeField]
    private List<GameObject> DescriptionsDV;
    private List<GameObject> lsDescriptions = new List<GameObject>();
    [HideInInspector]
    public List<string> lsNameButton = new List<string>();

    [Header("Setting")]

    private const string UILayerName = "UI1";
    public List<GameObject> uiObjects;
    private GameObject[] allPanels;

    public CinemachineVirtualCamera[] virtualCameras;
    private CinemachineVirtualCamera currentVcam;
    [SerializeField]
    private bool isTurnOnUI = true;
    [SerializeField]
    private bool isAutoPlayOn = false;
    [SerializeField]
    private bool isInfoPanelOn = false;
    [SerializeField]
    private bool isTutorialPanelOn = false;
    [SerializeField]
    private bool isFullScreen = false;
    private bool isClickedInfoButton = false;

    Vector3 targetScale;
    [SerializeField]
    private Image bgPanel;
    [SerializeField]
    private Button btnZoomIn;
    [SerializeField]
    private Button btnViewManNhan;
    [SerializeField]
    private Button btnViewManNhan1;
    [SerializeField]
    private Button btnSwithModelDV;
    [SerializeField]
    private Button btnSwithModelTV;
    [SerializeField]
    private List<GameObject> lsViewTagManNhan;
    [SerializeField]
    private GameObject objModelTVFull;
    [SerializeField]
    private GameObject objModelDVFull;
    [SerializeField]
    private GameObject objModelTVHalf;
    [SerializeField]
    private GameObject objModelDVHalf;
    [SerializeField]
    private GameObject swipeLucLap;
    [SerializeField]
    private GameObject swipeLysosome;
    [SerializeField]
    private GameObject swipeTrungThe;
    [SerializeField]
    private GameObject _objDescriptionTV;
    [SerializeField]
    private GameObject _objDescriptionDV;
    [SerializeField]
    private Button btnReturn;
    [SerializeField]
    private Button btnReturn1;
    private void SetNameObjectButtons()
    {
        for (int i = 0; i < groupButtonLeft.Length; i++)
        {
            groupButtonLeft[i].gameObject.SetActive(false);
        }
        Debug.Log($"controller.Buttons.Count: {controller.lsButtons.Count} groupButtonLeft: {groupButtonLeft.Length} lsNameButton: {lsNameButton.Count}");
        for (int i = 1; i < controller.lsButtons.Count; i++)
        {
            var num = i;

            groupButtonLeft[i].GetComponent<UIButton>().nameButton.text = num + ". " + lsNameButton[i];
            groupButtonLeft[i].name = lsNameButton[i];
            groupButtonLeft[i].gameObject.SetActive(true);

        }
    }
    private void SetNameListButtons()
    {



        for (int i = 0; i < controller.lsButtons.Count; i++)
        {
            var num = i;
            int index = i; // Capture index for the closure
            controller.lsButtons[i].onClick.RemoveAllListeners();
            controller.lsButtons[i].onClick.AddListener(() => OnButtonClicked(index, true));
            groupButtonLeft[i].onClick.RemoveAllListeners();
            groupButtonLeft[i].onClick.AddListener(() => OnButtonClicked(index, true));
            controller.lsButtons[i].GetComponent<UIButton>().numberButton.text = num.ToString();
            controller.lsButtons[i].GetComponent<UIButton>().nameButton.text = lsNameButton[index].ToString();


        }
    }
    void Awake()
    {
        _objDescriptionDV.gameObject.SetActive(false);
        _objDescriptionTV.gameObject.SetActive(true);
        _testScroll = _testScrollTV;
        lsNameButton.Clear();
        lsNameButton.AddRange(nameButtonList);
        lsDescriptions.Clear();
        lsDescriptions.AddRange(Descriptions);
        OnButtonReset();

        SetNameObjectButtons();
        CutModelBtn.onClick.AddListener(ToggleCutModel);
        // NextButton.onClick.AddListener(() => OnButtonNextPre(1));
        // PreviousButton.onClick.AddListener(() => OnButtonNextPre(-1));

        if (lsNameButton.Count < controller.lsButtons.Count)
        {
            foreach (var button in controller.lsButtons)
            {
                lsNameButton.Add(button.name);
            }
        }
        targetScale = Vector3.one * 0.0015f;
        SetNameListButtons();

        FullScreenBtn.onClick.AddListener(ToggleFullscreen);
        ExitBtn.onClick.AddListener(ExitApplication);
        InforBtn.onClick.AddListener(OnInfoButtonClicked);//(() => TogglePanel(uiInforPanel, ref isInfoPanelOn, InforBtn.GetComponent<UIButtonItem>()));
        TutorialBtn.onClick.AddListener(OnTutorialButtonClicked);//(() => TogglePanel(TutorialPanel, ref isTutorialPanelOn, TutorialBtn.GetComponent<UIButtonItem>()));
        AutoPlayBtn.onClick.AddListener(OnAutoPlayClicked);
        ResetBtn.onClick.AddListener(OnButtonReset);
        TurnUIBtn.onClick.AddListener(() => ToggleUI(ref isTurnOnUI));
        ShowDropdownBtn.onClick.AddListener(() => ToggleDropdown());
        uiObjects = FindObjectsWithLayer(LayerMask.NameToLayer(UILayerName)).ToList();
        uiObjects.Add(DescriptionPanel);
        allPanels = new GameObject[] { uiInforPanel, TutorialPanel };
        StartCoroutine(CountToShow());
        btnReturn.onClick.AddListener(OnClickReturnManNhan);
        btnReturn1.onClick.AddListener(OnClickReturnManNhan);

        foreach (var button in groupButtonLeft)
        {
            button.onClick.AddListener(() => { ButtonSelectGroup(button); });
        }
        //foreach (var item in lsMeshRenderer)
        //{
        //    item.material = materialNormal;
        //}
        btnZoomIn.onClick.AddListener(OnDeActiveBGPanel);
        btnViewManNhan.onClick.AddListener(OnViewManNhanTVClicked);
        btnViewManNhan1.onClick.AddListener(OnViewManNhanDVClicked);

        btnSwithModelDV.onClick.AddListener(OnSwithModelDVClicked);
        btnSwithModelTV.onClick.AddListener(OnSwithModelTVClicked);
        TurnOnUIViewDefault();
        StartCoroutine(DelayShow());
    }
    IEnumerator DelayShow()
    {
        yield return new WaitUntil(() => controller.isCoroutineRunning == false);
        OnButtonNextPre(0);

    }
    private void OnClickReturnManNhan()
    {
        for (int i = 1; i < controller.lsButtons.Count; i++)
        {
            controller.lsButtons[i].gameObject.SetActive(true);
        }
        foreach (var item in lsViewTagManNhan)
        {
            item.gameObject.SetActive(false);
        }
        lsDescriptions[1].SetActive(true);
        lsDescriptions[0].SetActive(false);
    }
    private void TurnOnUIViewDefault()
    {
        var ui = TurnUIBtn.GetComponent<UIToggleButton>();
        if (ui != null)
        {
            ui.isOn = true;
        }
        else
        {
            Debug.Log("Not Found UIToggleButton");
        }
    }
    public bool isClickUI = false;
    public bool isOnTarget = false;
    private void Update()
    {
        if (!isOnTarget)
        {
            CheckCameraView();
        }
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            isOnTarget = false;
        }
        MaintainButtonSize();
        DisableScrollRect();
        foreach (var button in totalButtons)
        {
            if (controller.isCoroutineRunning || cameraControl.isPanning || cameraControl.isZooming)
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
        }
        foreach (var button in controller.lsButtons)
        {
            if (controller.isCoroutineRunning || cameraControl.isPanning || cameraControl.isZooming)
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
        }
        foreach (var button in groupButtonLeft)
        {
            if (controller.isCoroutineRunning || cameraControl.isPanning || cameraControl.isZooming)
                button.interactable = false;
            else
            {
                if (button.GetComponent<UIButtonItem>().isSelected || cameraControl.isPanning || cameraControl.isZooming)
                {
                    button.interactable = false;

                }
                else
                {
                    button.interactable = true;
                }

            }
        }
    }
    public bool IsTouchingOrZooming()
    {
        return (cameraControl.isPanning || cameraControl.isZooming);
    }
    public void DisableScrollRect()
    {
        //    if (controller.isCoroutineRunning || freeLookCamera.GetComponent<CameraControl>().isPanning || freeLookCamera.GetComponent<CameraControl>().isZooming)
        //{
        //        ScrollRect.horizontal = false;
        //    }

        //else
        //    {
        if (cameraControl.isPanning || cameraControl.isZooming)
        {
            ScrollRect.horizontal = false;
            ScrollRectDV.horizontal = false;
        }
        else
        {
            ScrollRect.horizontal = true;
            ScrollRectDV.horizontal = true;
        }


    }
    public void ButtonSelectGroup(Button uIButton)
    {
        foreach (var button in groupButtonLeft)
        {
            button.GetComponent<UIButtonItem>().SetDisableButton();
            //  button.GetComponent<UIButtonItem>().ButtonExit();
            if (button == uIButton)
            {
                button.GetComponent<UIButtonItem>().SetClickButton();
            }
        }
    }
    float time = 0.00000000000000000000000000000000000000001f;
    IEnumerator CountToShow()
    {
        controller.lsButtons[0].gameObject.SetActive(false);
        for (int i = 1; i < 10; i++)
        {
            foreach (var button in controller.lsButtons)
            {
                button.gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(time);
            foreach (var button in controller.lsButtons)
            {
                button.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(time);
        }

    }
    IEnumerator CountToShowDescriptionPannel(GameObject obj)
    {
        //for (int i = 0; i < 3; i++)
        //{
        //    // obj.gameObject.SetActive(false);
        //    layoutGroup.gameObject.SetActive(false);
        //    yield return new WaitForSeconds(time);
        //    layoutGroup.gameObject.SetActive(true);

        //    //obj.gameObject.SetActive(true);
        //    yield return new WaitForSeconds(time);
        //}
        yield return new WaitForSeconds(time);
        SetActiveDescriptionPannel(true);

    }
    public Transform DefaultPoint;
    private bool isClickedReset = false;
    private void ResetTag()
    {
        foreach (var item in lsViewTagManNhan)
        {
            item.gameObject.SetActive(false);
        }
        for (int i = 1; i < controller.lsButtons.Count; i++)
        {
            controller.lsButtons[i].gameObject.SetActive(true);
        }
    }
    void OnButtonReset()
    {
        btnSwithModelDV.gameObject.SetActive(true);
        btnSwithModelTV.gameObject.SetActive(false);
        ResetTag();
        OnDeActiveBGPanel();
        NotTargetPoint();
        if (isAutoPlayOn)
        {
            isClickedReset = true;
            // ToggleAutoPlay(ref isAutoPlayOn);
            AutoPlayBtn.GetComponent<UIToggleButton>().isOn = false;
            freeLookCamera.GetComponent<CameraMotionControls>().autoRotate = false;
        }

        controller.ResetPosition();
        SetActiveDescriptionPannel(false, true);

        CurrentButtonSelected = -1;

        //  nameButtonCurrent.text = "Chọn một bước";
        if (!isTurnOnUI)
        {
            TurnUIBtn.onClick.Invoke();
        }

        if (uiInforPanel.activeSelf)
        {
            InforBtn.onClick.Invoke();
            UpdateUIButton(false);
        }

        if (TutorialPanel.activeSelf)
        {
            TutorialBtn.onClick.Invoke();
        }
        if (DescriptionPanel.activeSelf)
        {
            SetActiveDescriptionPannel(false);
            UpdateUIButton(false);
        }
        if (Dropdown.activeSelf)
        {
            ToggleDropdown();
        }


        //foreach (var item in lsMeshRenderer)
        //{
        //    item.material = materialNormal;
        //}
        isActiveOpacity = false;
        _testScroll.SetPositionIndex(0, true);
        OnButtonNextPre(0);
        //controller.ChangeCamera(controller.lsButtons[0]);
    }

    void OnButtonNextPre(int direction)
    {
        // Adjust CurrentButtonSelected based on direction
        Color disabledColor = new Color(112f / 255f, 112f / 255f, 112f / 255f); // #707070
        // if (controller.isCoroutineRunning)
        // {
        //     Debug.Log("direction: " + direction);
        //     return;
        // }
        if (direction == 0)
        {
            ImgPreviousButton.color = new Color(1f, 1f, 1f, 0.5f); ;
            PreviousButton.interactable = false;
        }
        else
        {
            ImgPreviousButton.color = new Color(1f, 1f, 1f, 1f); ;
            PreviousButton.interactable = true;
        }
        if (direction == controller.lsButtons.Count - 1)
        {
            ImgNextButton.color = new Color(1f, 1f, 1f, 0.5f);
            NextButton.interactable = false;
        }
        else
        {
            ImgNextButton.color = new Color(1f, 1f, 1f, 1f);
            NextButton.interactable = true;
        }

        // if (CurrentButtonSelected < 0)
        // {
        //     CurrentButtonSelected = 0;

        //     return;
        // }

        // if (CurrentButtonSelected >= controller.Buttons.Count)
        // {
        //     CurrentButtonSelected = controller.Buttons.Count - 1;
        //     return;
        // }

        //OnButtonClicked(CurrentButtonSelected);
    }


    bool isActiveDropdown;
    void ToggleDropdown()
    {
        isActiveDropdown = Dropdown.activeSelf;
        Dropdown.SetActive(!isActiveDropdown);
        if (!isActiveDropdown)
        {
            if (CurrentButtonSelected >= 0 && CurrentButtonSelected < groupButtonLeft.Length - 1)
            {
                groupButtonLeft[CurrentButtonSelected].onClick.Invoke();
            }
        }
    }

    public string SetName(int index)
    {
        //nameButtonCurrent.text = controller.Buttons[index].GetComponent<UIButton>().nameButton.text = nameButtonList[index].ToString();
        return nameButtonCurrent.text;
    }


    void OnDropdownValueChanged(int index)
    {
        Debug.Log("Selected: " + controller.lsButtons[index].name);
        if (!controller.isCoroutineRunning)
        {
            CurrentButtonSelected = index;
            controller.lsButtons[index].onClick.Invoke();
        }
    }

    public void OnButtonClicked(int index, bool isClicked)
    {
        ResetTag();
        OnButtonNextPre(index);

        if (!controller.isCoroutineRunning)
        {
            isOnTarget = true;
            CurrentButtonSelected = index;
            uiInforPanel.SetActive(false);
            if (TutorialPanel.activeSelf)
            {
                TutorialBtn.onClick.Invoke();
            }
            Debug.Log("Button clicked index: " + (index));
            Debug.Log("button clicked name: " + controller.lsButtons[index].name);
            lsDescriptions[index].gameObject.SetActive(true);
            NameText.text = lsNameButton[index].ToString();
            for (int i = 0; i < lsDescriptions.Count; i++)
            {
                if (i == index)
                {
                    lsDescriptions[i].gameObject.SetActive(true);
                }
                else
                {
                    lsDescriptions[i].gameObject.SetActive(false);
                }
            }
            // DescriptionText.text = Descriptions[index].text;
            // SetName(index);
            controller.ChangeCamera(controller.lsButtons[index]);
            var uiButton = controller.lsButtons[CurrentButtonSelected].GetComponent<UIButton>();
            if ((uiButton != null))
            {
                uiButton.SetBackground(true);
            }

            ButtonSelectGroup(groupButtonLeft[index]);
            OnTargetPoint(index);
            if (isClicked)
            {
                _testScroll.SetPositionIndex(index, isClicked);

            }
            UpdateUIButton(true);
            if (index < 1)
            {
                SetActiveDescriptionPannel(false, true);
            }

        }

    }

    void OnTargetPoint(int index)
    {
        isOnTarget = true;
        for (int i = 0; i < controller.lsButtons.Count; i++)
        {
            if (i == CurrentButtonSelected)
            {
                var uiButton = controller.lsButtons[i].GetComponent<UIButton>();
                if (uiButton != null)
                {
                    uiButton.SetBackground(true);
                }
            }
            else
            {

                if (i <= 0)
                {
                    controller.lsButtons[i].gameObject.SetActive(false);
                }
                else
                {
                    var uiButton = controller.lsButtons[i].GetComponent<UIButton>();
                    if (uiButton != null)
                    {
                        uiButton.SetBackground(false);
                    }
                }

                //SetActiveDescriptionPannel(true);


            }
            //todo
            //if (i == 0)
            //{
            //    controller.Buttons[i].gameObject.SetActive(false);
            //    SetActiveDescriptionPannel(false);


            //}
        }
        if (index != 0)
        {
            StartCoroutine(CountToShowDescriptionPannel(DescriptionPanel));

        }
        // SetActiveDescriptionPannel(true);

    }
    public void NotTargetPoint()
    {
        controller.lsButtons[0].gameObject.SetActive(false);
        for (int i = 1; i < controller.lsButtons.Count; i++)
        {

            if (i == 0)
            {
                controller.lsButtons[i].gameObject.SetActive(false);
                if (DescriptionPanel.activeSelf)
                {
                    SetActiveDescriptionPannel(false);

                }
            }
            else
            {
                var uiButton = controller.lsButtons[i].GetComponent<UIButton>();
                if (uiButton != null)
                {
                    uiButton.SetBackground(false);
                }
                //SetActiveDescriptionPannel(true);

            }

        }
        // nameButtonCurrent.text = "Chọn một bước";

        //if (DescriptionPanel.activeSelf && !isClickedInfoButton)
        //{
        //    SetActiveDescriptionPannel(false, true);

        //}
    }

    void ToggleUI(ref bool isUIOn)
    {
        isUIOn = !isUIOn;
        foreach (var obj in uiObjects)
        {
            obj.SetActive(isUIOn);
        }


        //if (nameButtonCurrent.text != "Chọn một bước")
        //if (CurrentButtonSelected != 0)
        //{
        //    SetActiveDescriptionPannel(isUIOn);
        //}

        if (!isUIOn)
        {
            if (uiInforPanel.activeSelf)
            {
                InforBtn.onClick.Invoke();
            }
            if (TutorialPanel.activeSelf)
            {
                TutorialBtn.onClick.Invoke();
            }

            SetActiveDescriptionPannel(false, true);

        }
        else
        {
            if (CurrentButtonSelected < 1)
            {
                SetActiveDescriptionPannel(false, true);
            }
            else
            {
                SetActiveDescriptionPannel(true, true);
            }

        }
        // ImgNextButton.enabled = isUIOn;
        // ImgPreviousButton.enabled = isUIOn;
        // _imgSwipe.enabled = isUIOn;
        // PreviousButton.interactable = isUIOn;
        // NextButton.interactable = isUIOn;
        Swipe.SetActive(isUIOn);
        swipeDV.SetActive(isUIOn);
        int i = 0;
        foreach (var obj in controller.lsButtons)
        {
            if (i == 0)
            {
                obj.gameObject.SetActive(false);
            }
            else
            {
                obj.gameObject.SetActive(isUIOn);
            }
            i++;
        }
        foreach (var obj in lsViewTagManNhan)
        {
            obj.SetActive(false);
        }



    }
    IEnumerator DelayShowButtonInfo()
    {
        yield return new WaitForSeconds(0.5f);
        var uiButton = InforBtn.GetComponent<UIButtonItem>();
        bool isActive = DescriptionPanel.activeSelf || uiInforPanel.activeSelf;
        isInfoPanelOn = isActive;
        if (isActive)
        {
            uiButton.SetClickButton();
        }
        else
        {
            uiButton.SetDisableButton();
        }
        Debug.Log("DelayShowButtonInfo: " + isActive);
    }
    private Coroutine Coroutine;
    private void SetActiveDescriptionPannel(bool isActive, bool isUpdate = false)
    {
        //if (isActive)
        //{
        //    isInfoPanelOn = true;
        //}
        //Debug.Log("SetActiveDescriptionPannel: " + isActive + " update: " + isUpdate);

        DescriptionPanel.SetActive(isActive);
        if (isUpdate)
        {
            var uiButton = InforBtn.GetComponent<UIButtonItem>();
            if (isActive)
            {
                uiButton.SetClickButton();
            }
            else
            {
                uiButton.SetDisableButton();
            }
        }
        //if (Coroutine != null)
        //{
        //    StopCoroutine(Coroutine);
        //    Coroutine = null;
        //}
        //Debug.Log("start corutine");
        //Coroutine = StartCoroutine(DelayShowButtonInfo());



    }
    public void UpdateUIButton(bool isActive)
    {
        var uiButton = InforBtn.GetComponent<UIButtonItem>();
        if (isActive)
        {
            uiButton.SetClickButton();
        }
        else
        {
            uiButton.SetDisableButton();
        }
    }
    private void OnTutorialButtonClicked()
    {
        if (uiInforPanel.activeSelf)
        {
            InforBtn.onClick.Invoke();
        }
        if (DescriptionPanel.activeSelf)
        {
            SetActiveDescriptionPannel(false, true);
        }
        isTutorialPanelOn = !isTutorialPanelOn;
        TutorialPanel.SetActive(isTutorialPanelOn);

        var uiButton = TutorialBtn.GetComponent<UIButtonItem>();
        if (isTutorialPanelOn)
        {
            uiButton.SetClickButton();
        }
        else
        {
            uiButton.SetDisableButton();
        }
    }
    private void OnInfoButtonClicked()
    {
        if (TutorialPanel.activeSelf)
        {
            TutorialBtn.onClick.Invoke();
        }

        isInfoPanelOn = DescriptionPanel.activeSelf || uiInforPanel.activeSelf;
        //Debug.Log("isInfoPanelOn1: " + isInfoPanelOn);
        isInfoPanelOn = !isInfoPanelOn;
        // Debug.Log("isInfoPanelOn: " + isInfoPanelOn);
        if (!isInfoPanelOn)
        {
            SetActiveDescriptionPannel(false, true);
            uiInforPanel.SetActive(false);
            // Debug.Log("turnoff panel: ");

        }
        else
        {
            if (isOnTarget)
            {

                if (freeLookCamera.m_Lens.FieldOfView > 20)
                {
                    // Debug.Log("turnon panel istarget = true  fdv> 20 ");

                    isOnTarget = false;
                    SetActiveDescriptionPannel(false);
                    uiInforPanel.SetActive(true);
                }
                else
                {
                    // Debug.Log("turnon panel istarget = true  fdv < 20 ");
                    if (CurrentButtonSelected >= 1)
                    {
                        SetActiveDescriptionPannel(true, true);
                        uiInforPanel.SetActive(false);

                    }
                    else
                    {
                        SetActiveDescriptionPannel(false);
                        uiInforPanel.SetActive(true);
                        UpdateUIButton(true);
                    }
                }

            }
            else
            {
                // Debug.Log("turnon panel nha may ciment ");

                uiInforPanel.SetActive(true);
                SetActiveDescriptionPannel(false, false);
                UpdateUIButton(true);

            }
        }



        //if (DescriptionPanel.activeSelf)
        //{
        //    uiInforPanel.SetActive(false);
        //}
    }
    void TogglePanel(GameObject panel, ref bool isPanelOn, UIButtonItem buttonitem)
    {
        Debug.Log("Toogle: ");
        isPanelOn = buttonitem.isSelected;
        foreach (var pnl in allPanels)
        {
            if (pnl != panel)
            {
                pnl.SetActive(false);
            }
        }
        panel.SetActive(isPanelOn);
        if (uiInforPanel.activeSelf)
        {
            InforBtn.onClick.Invoke();
        }
        if (DescriptionPanel.activeSelf)
        {
            SetActiveDescriptionPannel(false, false);
        }
    }
    private void OnAutoPlayClicked()
    {
        isClickedReset = false;
        ToggleAutoPlay(ref isAutoPlayOn);
    }
    void ToggleAutoPlay(ref bool isAutoPlayOn)
    {
        if (!isClickedReset)
        {
            OnButtonReset();

        }
        isAutoPlayOn = !isAutoPlayOn;
        if (isAutoPlayOn)
        {
            // Turn off other toggle buttons
            //isInfoPanelOn = false;
            //isTutorialPanelOn = false;
            //isTurnOnUI = false;
            if (isTurnOnUI)
            {
                TurnUIBtn.onClick.Invoke();
            }
            //uiInforPanel.SetActive(false);
            //TutorialPanel.SetActive(false);
            if (uiInforPanel.activeSelf)
            {
                InforBtn.onClick.Invoke();
            }
            if (TutorialPanel.activeSelf)
            {
                TutorialBtn.onClick.Invoke();
            }
            if (Swipe.activeSelf)
            {
                Swipe.SetActive(false);
            }
            if (swipeDV.activeSelf)
            {
                swipeDV.SetActive(false);
            }
            SetActiveDescriptionPannel(false, true);
        }
        else
        {
            foreach (var obj in uiObjects)
            {
                obj.SetActive(true);
            }
            if (CurrentButtonSelected < 1)
            {
                DescriptionPanel.SetActive(false);
            }
            if (objModelTVFull.activeSelf || objModelTVHalf.activeSelf)
            {
                Swipe.SetActive(true);
            }
            else
            {
                swipeDV.SetActive(true);
            }
            if (!isTurnOnUI)
            {
                TurnUIBtn.onClick.Invoke();
            }
        }
        freeLookCamera.GetComponent<CameraMotionControls>().autoRotate = isAutoPlayOn;


    }
    bool isActiveOpacity = false;


    void ToggleCutModel()
    {
        if (objModelDVHalf.activeSelf)
        {
            objModelDVHalf.SetActive(false);
            objModelDVFull.SetActive(true);
            objModelTVFull.SetActive(false);
            objModelTVHalf.SetActive(false);
        }
        else if (objModelDVFull.activeSelf)
        {
            objModelDVHalf.SetActive(true);
            objModelDVFull.SetActive(false);
            objModelTVFull.SetActive(false);
            objModelTVHalf.SetActive(false);
        }
        else if (objModelTVHalf.activeSelf)
        {
            objModelDVHalf.SetActive(false);
            objModelDVFull.SetActive(false);
            objModelTVHalf.SetActive(false);
            objModelTVFull.SetActive(true);
        }
        else
        {
            objModelDVHalf.SetActive(false);
            objModelDVFull.SetActive(false);
            objModelTVHalf.SetActive(true);
            objModelTVFull.SetActive(false);
        }

    }

    void ToggleFullscreen()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
        if (isFullScreen)
        {
            // Set to the maximum resolution available in fullscreen
            Resolution maxResolution = Screen.resolutions.Last();
            Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        }
        else
        {
            // Set to windowed mode with a resolution of 1280x720
            Screen.SetResolution(1280, 720, false);
        }
    }


    void ExitApplication()
    {
        //foreach (var item in lsMeshRenderer)
        //{
        //    item.material = materialNormal;
        //}
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // This will stop play mode in the Unity Editor
#endif
    }

    GameObject[] FindObjectsWithLayer(int layer)
    {
        GameObject[] objects = FindObjectsOfType<GameObject>();
        List<GameObject> uiLayerObjects = new List<GameObject>();

        foreach (GameObject obj in objects)
        {
            if (obj.layer == layer)
            {
                uiLayerObjects.Add(obj);
            }
        }

        return uiLayerObjects.ToArray();
    }

    void SwitchCamera(int index)
    {
        if (currentVcam != null)
        {
            currentVcam.Priority = 5;
        }

        currentVcam = virtualCameras[index];
        currentVcam.Priority = 20;

        Debug.Log("Switched to camera: " + currentVcam.name);
    }

    private bool isCooldown = false;
    private float cooldownTime = 0.5f;
    IEnumerator ButtonCooldown(System.Action action)
    {
        if (!isCooldown)
        {
            isCooldown = true;
            action();
            yield return new WaitForSeconds(cooldownTime);
            isCooldown = false;
        }
    }

    void CheckCameraView()
    {


        if (freeLookCamera.m_Lens.FieldOfView > 20 && !controller.isCoroutineRunning)
        {
            //isOnTarget = false;
            if (DescriptionPanel.activeSelf)
            {
                SetActiveDescriptionPannel(false, true);
            }
            NotTargetPoint();
            //nameButtonCurrent.text = "CHỌN MỘT BƯỚC";
        }

    }
    void MaintainButtonSize()
    {
        // Reference FOV where the button size is correct
        float referenceFov = 60.0f;

        // Get the current FOV of the camera
        float currentFov = Camera.main.fieldOfView;

        // Calculate the scale factor based on the FOV ratio
        float scaleFactor = currentFov / referenceFov;

        // Apply the calculated scale factor to each button
        foreach (var button in controller.lsButtons)
        {
            Vector3 targetScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            button.transform.localScale = Vector3.Lerp(button.transform.localScale, targetScale, Time.deltaTime * 5);
        }
    }
    public void DropdownValueChange()
    {
        foreach (var button in groupButtonLeft)
        {
            button.interactable = false;
        }
        StartCoroutine(DayLayDropdownActive());

    }
    IEnumerator DayLayDropdownActive()
    {
        yield return new WaitUntil(() => controller.isCoroutineRunning == false);
        foreach (var button in groupButtonLeft)
        {
            button.interactable = true;
        }
    }
    public void SetActiveBGPanel(Sprite sprImg)
    {
        bgPanel.sprite = sprImg;
        bgPanel.gameObject.SetActive(true);
    }
    public void OnDeActiveBGPanel()
    {
        bgPanel.gameObject.SetActive(false);
    }
    public void OnViewManNhanTVClicked()
    {
        for (int i = 1; i < controller.lsButtons.Count; i++)
        {
            controller.lsButtons[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < lsViewTagManNhan.Count; i++)
        {
            if (i < 3)
            {
                lsViewTagManNhan[i].gameObject.SetActive(true);

            }
            else
            {
                lsViewTagManNhan[i].gameObject.SetActive(false);
            }
        }
        lsDescriptions[1].SetActive(false);
        lsDescriptions[0].SetActive(true);


    }
    public void OnViewManNhanDVClicked()
    {
        for (int i = 1; i < controller.lsButtons.Count; i++)
        {
            controller.lsButtons[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < lsViewTagManNhan.Count; i++)
        {
            if (i < 3)
            {
                lsViewTagManNhan[i].gameObject.SetActive(false);

            }
            else
            {
                lsViewTagManNhan[i].gameObject.SetActive(true);
            }
        }
        lsDescriptions[1].SetActive(false);
        lsDescriptions[0].SetActive(true);


    }
    public void OnSwithModelDVClicked()
    {
        _objDescriptionDV.gameObject.SetActive(true);
        _objDescriptionTV.gameObject.SetActive(false);

        Swipe.SetActive(false);
        swipeDV.SetActive(true);
        _testScroll = _testScrollDV;

        lsNameButton.Clear();
        lsNameButton.AddRange(nameButtonListDV);
        lsDescriptions.Clear();
        lsDescriptions.AddRange(DescriptionsDV);

        objModelDVFull.gameObject.SetActive(false);
        objModelTVFull.gameObject.SetActive(false);
        objModelTVHalf.gameObject.SetActive(false);
        objModelDVHalf.gameObject.SetActive(true);
        btnSwithModelDV.gameObject.SetActive(false);
        btnSwithModelTV.gameObject.SetActive(true);
        controller.GetListDataDV();
        SetNameObjectButtons();
        SetNameListButtons();


        if (CurrentButtonSelected < 0)
        {
            CurrentButtonSelected = 0;
        }

        OnButtonClicked(CurrentButtonSelected, true);
        //OnButtonReset(false);

    }
    public void OnSwithModelTVClicked()
    {
        _objDescriptionDV.gameObject.SetActive(false);
        _objDescriptionTV.gameObject.SetActive(true);
        Swipe.SetActive(true);
        swipeDV.SetActive(false);
        _testScroll = _testScrollTV;
        lsNameButton.Clear();
        lsNameButton.AddRange(nameButtonList);
        lsDescriptions.Clear();
        lsDescriptions.AddRange(Descriptions);

        objModelDVFull.gameObject.SetActive(false);
        objModelTVFull.gameObject.SetActive(false);
        objModelDVHalf.gameObject.SetActive(false);
        objModelTVHalf.gameObject.SetActive(true);
        btnSwithModelDV.gameObject.SetActive(true);
        btnSwithModelTV.gameObject.SetActive(false);


        controller.GetListDataTV();
        SetNameObjectButtons();
        SetNameListButtons();

        if (CurrentButtonSelected >= groupButtonLeft.Length - 1)
        {
            CurrentButtonSelected -= 1;
        }
        if (CurrentButtonSelected < 0)
        {
            CurrentButtonSelected = 0;
        }
        OnButtonClicked(CurrentButtonSelected, true);

    }
}




