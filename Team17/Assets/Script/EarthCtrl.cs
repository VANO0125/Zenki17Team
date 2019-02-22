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
        //HP
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
	{
       if(hp<=0)//HP0以下の時
        {
            //破壊
            Destroy(gameObject);
        }
    }
    public void Damage(float damage)
    {
        //ダメージ
        hp -= damage;
        star.FallMeteo();
    }

    public void AddScore(int score)
    {
        this.score += score;//加点
        scoreNumber.Set(this.score);//スコアを更新
        
        this.score += score;
        star.FallStar();
    }
}
