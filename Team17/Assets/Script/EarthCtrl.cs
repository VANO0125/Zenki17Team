using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    private float hp;
    public float maxHp;
    private float score;
    public ShootingStar star;

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
        star.FallMeteo();
    }

    public void AddScore(float score)
    {
        this.score += score;
        star.FallStar();
    }
}
