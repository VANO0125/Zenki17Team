using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImetmCatcher : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    private MeteoCtrl meteo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggetEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Meteo")
        {
            MeteoCtrl meteo = col.gameObject.GetComponent<MeteoCtrl>();
            if (!meteo.isShot)
            {
                meteo.target = player;
                meteo.speed = 10;
            }
        }
    }
}
