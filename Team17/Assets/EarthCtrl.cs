using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    public float hp;
    public float maxHp;
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
}
