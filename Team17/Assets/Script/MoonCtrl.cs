using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonCtrl : MonoBehaviour
{

    public Transform earth;//地球の位置
    public float speed = 1;//公転スピード

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (earth != null)
            transform.RotateAround(earth.position, Vector3.forward, speed * 0.1f);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Meteo")
        {
            MeteoCtrl meteo = col.GetComponent<MeteoCtrl>();
            if (meteo.size <= 1)
            {
                Rigidbody2D rig = col.gameObject.GetComponent<Rigidbody2D>();
                rig.velocity = (transform.position - col.transform.position) * 3;
            }
        }
    }
}
