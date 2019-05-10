using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Waring : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> waring;
    [SerializeField]
    private RectTransform canvasRectTfm;

    // Start is called before the first frame update
    void Start()
    {
        CheckScreenOut(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //CheckScreenOut(transform.position);
    }

    void CheckScreenOut(Vector3 _pos)
    {
        Vector3 view_pos = Camera.main.WorldToViewportPoint(_pos);
        if (view_pos.x < -0.0f)
        {
            waring[0] = GameObject.Find("Canvas/WaringLeft");
            waring[0].GetComponent<Image>().enabled = true;
            StartCoroutine("TextOff", 0);
        }
        else if (view_pos.x > 1.0f)
        {
            waring[1] = GameObject.Find("Canvas/WaringRight");
            waring[1].GetComponent<Image>().enabled = true;
            StartCoroutine("TextOff", 1);
        }
        else if (view_pos.y > 1.0f)
        {
            waring[2] = GameObject.Find("Canvas/WaringTop");
            Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 tfpos = waring[2].transform.position;
            tfpos.x = pos.x;
            waring[2].transform.position = tfpos;
            waring[2].GetComponent<Image>().enabled = true;
            StartCoroutine("TextOff", 2);
        }

        //if (view_pos.x < -0.0f ||
        //   view_pos.x > 1.0f ||
        //   view_pos.y < -0.0f ||
        //   view_pos.y > 1.0f)
        //{
        //    // 範囲外 
        //    Debug.Log("外だよ");
        //}
        // 範囲内 
        return;
    }

    IEnumerator TextOff(int i)
    {
        yield return new WaitForSeconds(3.5f);
        waring[i].GetComponent<Image>().enabled = false;
        yield break;
    }
}
