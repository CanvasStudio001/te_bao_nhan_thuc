using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour
{
    [SerializeField] private UIMain _uiMain;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform content;
    [SerializeField] private RectTransform[] items;

    [SerializeField] private HorizontalLayoutGroup _layoutGroup;

    public ScrollViewItem _itemInstance;
    public Color[] colors;
    public GameObject scrollbar;
    private float scroll_pos = 0;
    float[] pos;
    private bool runIt = false;
    private float time;
    private Button takeTheBtn;
    int btnNumber;
    private Scrollbar _scrollBar;

    private bool isLoaded = false;
    // Start is called before the first frame update
    void Start()
    {
        _scrollBar = scrollbar.GetComponent<Scrollbar>();
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitUntil(() => _uiMain != null);

        int lenght = _uiMain.lsNameButton.Count;
        items = new RectTransform[lenght + 1];
        var a = SpawnItem("Chọn một bước");
        items[0] = a.gameObject.GetComponent<RectTransform>();
        for (int i = 1; i < lenght; i++)
        {
            a = SpawnItem(_uiMain.SetName(i));
            items[i] = a.gameObject.GetComponent<RectTransform>();
        }
        isLoaded = true;
    }
    private ScrollViewItem SpawnItem(string content)
    {
        var spawnItem = Instantiate(_itemInstance, Vector3.zero, Quaternion.identity);
        spawnItem.transform.SetParent(this.transform);
        spawnItem.SetContent(content);
        spawnItem.transform.localScale = Vector3.one;
        spawnItem.gameObject.SetActive(true);

        return spawnItem;

    }
    // Update is called once per frame
    void Update()
    {
        return;
        if (!isLoaded)
        {
            return;
        }
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);


        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            scroll_pos = _scrollBar.value;
            //_uiMain.IsClickSwipe = true;
        }
        else
        {
            //   _uiMain.IsClickSwipe = false;

            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2.0f) && scroll_pos > pos[i] - (distance / 2.0f))
                {
                    _scrollBar.value = Mathf.Lerp(_scrollBar.value, pos[i], 0.15f);
                }
            }
        }


    }


    public void ChangeStage(int index)
    {
        btnNumber += index;
        // if (btnNumber < 0)
        // {
        //     btnNumber = 0;

        // }
        // else if (btnNumber > pos.Length - 1)
        // {
        //     btnNumber = pos.Length - 1;
        // }
        // time = 0;
        // scroll_pos = (pos[btnNumber]);
        // runIt = true;
        // _layoutGroup.enabled = false;
        //_uiMain.OnButtonClicked(btnNumber);
        //_layoutGroup.enabled = true;
        CenterOnItem(btnNumber);
    }

    public void CenterOnItem(int index)
    {
        if (index < 0 || index >= items.Length)
        {
            Debug.LogWarning("Index out of bounds!");
            return;
        }
        RectTransform targetItem = items[index];

        // Calculate the position of the item relative to the content
        Vector3 itemLocalPosition = targetItem.localPosition;

        // Calculate the content size and the viewport size
        float contentWidth = content.rect.width;
        float viewportWidth = _scrollRect.viewport.rect.width;

        // Calculate the target position to center the item
        float targetPositionX = -itemLocalPosition.x + (viewportWidth / 2) - (targetItem.rect.width / 2);

        // Clamp the target position to ensure it stays within bounds
        float clampedX = Mathf.Clamp(targetPositionX, -(contentWidth - viewportWidth), 0f);

        // Set the content position
        content.localPosition = new Vector3(clampedX, content.localPosition.y, content.localPosition.z);

        Debug.Log($"index: {index} normalizedPosition: {content.localPosition}");
        // Apply the position to the scroll rect
        //_scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(normalizedPositionX);

    }
}