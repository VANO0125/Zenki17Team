using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarthCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    private float hp;
    public float maxHp;
    private int score;//スコア
    public Number scoreNumber;//スコア描写
    public bool isDead;//死亡判定

	public ShootingStar star;//地上視点
    public GameObject earthCamera;//地上カメラ
    private bool isDisplay;//地上カメラが表示されているか
    public float earthTimer = 120f;//地上カメラを表示する時間
    private float timer;

    void Start()
    {
        //HP
        hp = maxHp;
        timer = earthTimer;
    }

    // Update is called once per frame
    void Update()
	{
        DisPlayCamera();
       if(hp<=0)//HP0以下の時
        {
            isDead = true;
            Destroy();
        }
    }

    void Destroy()
    {
        //破壊処理
        Destroy(gameObject);
    }

    public void Damage(float damage)
    {
        //ダメージ
        isDisplay = true;//カメラ表示
        timer = earthTimer;//表示時間をリセット
        hp -= damage;
        star.FallMeteo();
    }

    public void AddScore(int score)
    {
        isDisplay = true;//カメラ表示
        timer = earthTimer;//表示時間をリセット
        this.score += score;//加点
        scoreNumber.Set(this.score);//スコアを更新
        star.FallStar();
    }

    private void DisPlayCamera()
    {
        //地上カメラを一定時間表示する処理
        if (isDisplay)
        {
            earthCamera.SetActive(true);
            timer--;
        }
        if (timer <= 0)
        {
            isDisplay = false;
            earthCamera.SetActive(false);
        }
        
    }
}
