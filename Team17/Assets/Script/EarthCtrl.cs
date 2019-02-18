using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    public float hp;
    public float maxHp;
    public GameObject ryusei;
    void Start()
    {
        hp = maxHp;
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Damage(float damage)
    {
        hp -= damage;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag=="Meteo")
        {
            Instantiate(ryusei,new Vector3(25,23,-1), transform.rotation);
        }

    }
}
