using UnityEngine;

public class CheckActive : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("OnEnable");
    }
    private void OnDisable()
    {
        Debug.Log("OnDisable");

    }
}
