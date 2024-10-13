using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class swipe : MonoBehaviour
{
    public ScrollViewItem _itemInstance;
    public Color[] colors;
    public GameObject scrollbar;
    private float scroll_pos = 0;
    float[] pos;
    private bool runIt = false;
    private float time;
    private Button takeTheBtn;
    int btnNumber;
    private bool isLoaded = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(5.0f);
        for (int i = 0; i < 10; i++)
        {
            SpawnItem("Chon Mot Buoc " + i);
        }
        isLoaded = true;
    }
    private ScrollViewItem SpawnItem(string content)
    {
        var spawnItem = Instantiate(_itemInstance, Vector3.zero, Quaternion.identity);
        spawnItem.transform.SetParent(this.transform);
        spawnItem.SetContent(content);
        spawnItem.gameObject.SetActive(true);
        return spawnItem;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLoaded)
        {
            return;
        }
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);

        if (runIt)
        {
            GecisiDuzenle(distance, pos, takeTheBtn);
            time += Time.deltaTime;

            if (time > 1f)
            {
                time = 0;
                runIt = false;
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }


        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                //imageContent.transform.GetChild(i).localScale = Vector2.Lerp(imageContent.transform.GetChild(i).localScale, new Vector2(1.2f, 1.2f), 0.1f);
                // imageContent.transform.GetChild(i).GetComponent<Image>().color = colors[1];
                for (int j = 0; j < pos.Length; j++)
                {
                    if (j != i)
                    {
                        // imageContent.transform.GetChild(j).GetComponent<Image>().color = colors[0];
                        // imageContent.transform.GetChild(j).localScale = Vector2.Lerp(imageContent.transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                        transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }


    }

    private void GecisiDuzenle(float distance, float[] pos, Button btn)
    {
        // btnSayi = System.Int32.Parse(btn.transform.name);

        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[btnNumber], 1f * Time.deltaTime);

            }
        }

        // for (int i = 0; i < btn.transform.parent.transform.childCount; i++)
        // {
        //     btn.transform.name = ".";
        // }

    }
    public void WhichBtnClicked(Button btn)
    {
        btn.transform.name = "clicked";
        for (int i = 0; i < btn.transform.parent.transform.childCount; i++)
        {
            if (btn.transform.parent.transform.GetChild(i).transform.name == "clicked")
            {
                btnNumber = i;
                takeTheBtn = btn;
                time = 0;
                scroll_pos = (pos[btnNumber]);
                runIt = true;
            }
        }


    }
    public void ChangeStage(int index)
    {
        btnNumber += index;
        //takeTheBtn = btn;
        if (btnNumber < 0)
        {
            btnNumber = 0;

        }
        else if (btnNumber > pos.Length - 1)
        {
            btnNumber = pos.Length - 1;
        }
        time = 0;
        scroll_pos = (pos[btnNumber]);
        runIt = true;
    }

}