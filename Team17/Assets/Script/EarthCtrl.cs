using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarthCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    private float hp;
    public float maxHp;
    private int score;
	public ShootingStar star;
    public Number scoreNumber;

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

    public void AddScore(int score)
    {
        this.score += score;//加点
        scoreNumber.Set(this.score);//スコアを更新
        star.FallStar();
    }
}
