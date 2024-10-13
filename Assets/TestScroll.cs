using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestScroll : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{

    [SerializeField] UIMain _uiMain;
    [SerializeField] ScrollViewItem[] arrScrollView;
    public GameObject scrollbar;
    float scroll_pos = 0;
    float[] pos;
    [SerializeField]
    public int posIndex = 0;
    [SerializeField]
    int _currentPosIndex = 0;
    float distance;
    public float speedMove = 5f;
    private Scrollbar _scrollBar;
    private void Start()
    {
        pos = new float[transform.childCount];
        distance = 1f / (pos.Length - 1f);
        _scrollBar = scrollbar.GetComponent<Scrollbar>();
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

    }
    public void Next()
    {
        if (_uiMain.controller.isCoroutineRunning == false)
        {
            if (posIndex < pos.Length - 1)
            {
                posIndex += 1;
                scroll_pos = pos[posIndex];
                _uiMain.OnButtonClicked(posIndex, false);
                _currentPosIndex = posIndex;
            }
        }
    }
    public void Preview()
    {
        if (_uiMain.controller.isCoroutineRunning == false)
        {
            if (posIndex > 0)
            {
                posIndex -= 1;
                scroll_pos = pos[posIndex];
                _uiMain.OnButtonClicked(posIndex, false);
                _currentPosIndex = posIndex;

            }
        }

    }
    public void SetPositionIndex(int index, bool isClicked)
    {
        if (pos == null)
        {
            return;
        }
        if (index < 0)
        {
            index = 0;
        }
        else if (index > pos.Length - 1)
        {
            index = pos.Length - 1;
        }

        posIndex = index;
        scroll_pos = pos[posIndex];
        _currentPosIndex = posIndex;
        if (!isClicked)
        {
            _uiMain.OnButtonClicked(posIndex, isClicked);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (_scrollBar == null)
        {
            return;
        }




        if (Input.GetMouseButton(0))
        {
            scroll_pos = _scrollBar.value;

        }
        else
        {

            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    _scrollBar.value = Mathf.Lerp(_scrollBar.value, pos[i], speedMove * Time.deltaTime);
                    posIndex = i;
                    if (_currentPosIndex != posIndex)
                    {
                        SetPositionIndex(posIndex, false);
                    }
                }
            }
        }


    }



    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // _uiMain.IsClickSwipe = true;

        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {


        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // _uiMain.IsClickSwipe = false;

    }
}
