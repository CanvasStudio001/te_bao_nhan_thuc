using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBallManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _objBall;
    [SerializeField]
    private Transform _parent;
    private List<GameObject> lsBall = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelaySpawnBall());
    }
    private GameObject _objInit;
    private int numberRandom;
    IEnumerator DelaySpawnBall()
    {
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < 200; i++)
        {
            _objInit = Instantiate(_objBall, _objBall.transform.position, Quaternion.identity);
            _objInit.transform.SetParent(_parent);
            // _objInit.transform.position = _objBall.transform.position;
            // _objInit.transform.localPosition = _objBall.transform.localPosition;
            numberRandom = 6;
            if (i > 50)
            {
                numberRandom = 3;
            }
            _objInit.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f) * numberRandom;

            _objInit.SetActive(true);
            lsBall.Add(_objInit);
            yield return new WaitForSeconds(0.02f);
        }

    }
}
