using UnityEngine;
using UnityEngine.EventSystems;

public class CheckMouseClicked : MonoBehaviour
{
    public EventTrigger eventTrigger;
    public UIMain _uiMain;
    void Start()
    {
        eventTrigger = gameObject.AddComponent<EventTrigger>();

        // Create entry for Begin Drag
        EventTrigger.Entry beginDragEntry = new EventTrigger.Entry();
        beginDragEntry.eventID = EventTriggerType.BeginDrag;
        beginDragEntry.callback.AddListener((data) => { OnBeginDrag((PointerEventData)data); });
        eventTrigger.triggers.Add(beginDragEntry);

        // Create entry for Drag
        EventTrigger.Entry dragEntry = new EventTrigger.Entry();
        dragEntry.eventID = EventTriggerType.Drag;
        dragEntry.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
        eventTrigger.triggers.Add(dragEntry);

        // Create entry for End Drag
        EventTrigger.Entry endDragEntry = new EventTrigger.Entry();
        endDragEntry.eventID = EventTriggerType.EndDrag;
        endDragEntry.callback.AddListener((data) => { OnEndDrag((PointerEventData)data); });
        eventTrigger.triggers.Add(endDragEntry);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log("Drag started on dropdown panel.");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_uiMain.IsTouchingOrZooming())
        {
            _uiMain.IsClickSwipe = false;
        }
        else
        {
            _uiMain.IsClickSwipe = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //  Debug.Log("Drag ended on dropdown panel.");
        _uiMain.IsClickSwipe = false;

    }
}
