using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DropdownStateTracker : MonoBehaviour
{
    public GameObject tmpDropdown; // Reference to the Dropdown component
    public Image Arrow;
    public float rotationSpeed = 1.0f; // Speed of rotation
    private bool isDropdownOpen = false;
    private Coroutine rotateCoroutine;
 



    void Update()
    {
        if (tmpDropdown.gameObject.activeSelf && !isDropdownOpen)
        {
            isDropdownOpen = true;
            // Perform any additional actions when the dropdown is opened
            if (rotateCoroutine != null)
            {
                StopCoroutine(rotateCoroutine);
            }
            rotateCoroutine = StartCoroutine(RotateArrow(0f));
        }
        else if (!tmpDropdown.gameObject.activeSelf && isDropdownOpen)
        {
            isDropdownOpen = false;
            // Perform any additional actions when the dropdown is closed
            if (rotateCoroutine != null)
            {
                StopCoroutine(rotateCoroutine);
            }
            rotateCoroutine = StartCoroutine(RotateArrow(180f));
        }

    }

  
    private IEnumerator RotateArrow(float targetZRotation)
    {
        Quaternion startRotation = Arrow.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(
            Arrow.transform.eulerAngles.x,
            Arrow.transform.eulerAngles.y,
            targetZRotation
        );

        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * rotationSpeed;
            Arrow.transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime);
            yield return null;
        }

        Arrow.transform.rotation = endRotation;
    }
}
