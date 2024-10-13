using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurObjectBehindCamera : MonoBehaviour
{ 
    public Transform targetObject; // Object mà bạn muốn nhìn thấy
    public Material wallMaterial;  // Material của bức tường
    public float transparentAlpha = 0.2f; // Độ trong suốt khi bức tường mờ
    public float solidAlpha = 1.0f; // Độ trong suốt khi bức tường không mờ

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Ray ray = new Ray(mainCamera.transform.position, targetObject.position - mainCamera.transform.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform != targetObject && hit.transform.gameObject.CompareTag("Model"))
            {
                wallMaterial.SetFloat("_Alpha", transparentAlpha);
            }
            else
            {
                wallMaterial.SetFloat("_Alpha", solidAlpha);
            }
        }
        else
        {
            wallMaterial.SetFloat("_Alpha", solidAlpha);
        }
    }
}

