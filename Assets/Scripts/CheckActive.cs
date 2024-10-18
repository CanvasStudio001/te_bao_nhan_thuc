using UnityEngine;

public class CheckActive : MonoBehaviour
{
    private void OnEnable()
    {
        ;
        Debug.Log("OnEnable: " + this.gameObject.name);
    }
    private void OnDisable()
    {
        Debug.Log("OnDisable: " + this.gameObject.name);

    }
}
