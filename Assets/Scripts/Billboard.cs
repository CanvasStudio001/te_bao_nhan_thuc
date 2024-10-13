using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform cam;
    public Transform parentObject; // Đối tượng cha chứa các con
    private int originalIndex1; // Chỉ số ban đầu của đối tượng con đầu tiên
    private int originalIndex2; // Chỉ số ban đầu của đối tượng con thứ hai
    public bool swapped = false; // Biến kiểm tra trạng thái hoán đổi
    public RectTransform uiElement;

    void Start()
    {
        parentObject = this.transform; // Đảm bảo parentObject là đối tượng chứa script này
        uiElement = GetComponent<RectTransform>();
        if (parentObject.childCount >= 2)
        {
            // Lưu trữ chỉ số ban đầu của hai đối tượng con đầu tiên
            originalIndex1 = parentObject.GetChild(0).GetSiblingIndex();
            originalIndex2 = parentObject.GetChild(1).GetSiblingIndex();
        }

    }

    void SwapChildOrder()
    {
        // Lấy hai đối tượng con đầu tiên
        Transform child1 = parentObject.GetChild(0);
        Transform child2 = parentObject.GetChild(1);

        // Đổi thứ tự
        child1.SetSiblingIndex(1);
        child2.SetSiblingIndex(0);
    }

    void RestoreOriginalOrder()
    {
        // Đưa các đối tượng con trở về thứ tự ban đầu
        Transform child1 = parentObject.GetChild(0);
        Transform child2 = parentObject.GetChild(1);

        // Đặt lại chỉ số ban đầu
        child1.SetSiblingIndex(originalIndex1);
        child2.SetSiblingIndex(originalIndex2);
    }
    IEnumerator CountToShow()
    {
        this.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        SwapPivot();
        SwapChildOrder();
        this.GetComponent<CanvasGroup>().DOFade(1, 0.5f);

    }
    void SwapPivot()
    {// Get the current pivot
        Vector2 currentPivot = uiElement.pivot;

        // Toggle the x value of the pivot
        currentPivot.x = currentPivot.x == 0 ? 1 : 0;

        // Apply the new pivot
        uiElement.pivot = currentPivot;
    }
    void Update()
    {
        transform.LookAt(transform.position + cam.forward);

        if (parentObject.childCount < 2) return;

        // Lấy góc quay hiện tại của đối tượng cha trên trục Y
 
        // Kiểm tra điều kiện: Quay ngược về sau khoảng 3/4 (270 độ)
        if (cam.transform.position.z > 250)
        {
            // Nếu chưa hoán đổi, thực hiện hoán đổi thứ tự của hai đối tượng con đầu tiên
            if (!swapped)
            {
             
                StartCoroutine(CountToShow());    
                swapped = true;
            }
        }
        else
        {
            // Nếu đã hoán đổi và điều kiện không còn thỏa mãn, khôi phục thứ tự ban đầu
            if (swapped)
            {
        
                StartCoroutine(CountToShow());
                swapped = false;
            }
        }
    }
}
