using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestEnable : MonoBehaviour
{
    void OnEnable()
    {
        Debug.Log("Onennable");
    }
    private void OnDisable()
    {
        Debug.Log("OnDisable");
    }
}
