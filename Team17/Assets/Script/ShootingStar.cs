using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStar : MonoBehaviour
{
    public GameObject ryusei, meteo;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FallStar()
    {
        //流れ星を2秒表示
        float r = Random.Range(-0.7f, 0.3f);
        GameObject obj = Instantiate(ryusei, transform.position + Vector3.up * r, Quaternion.identity);
        Destroy(obj, 2f);
    }

    public void FallMeteo()
    {
        //隕石を2秒表示
        GameObject obj = Instantiate(meteo, transform.position, Quaternion.identity);
        Destroy(obj, 2f);
    }
}
