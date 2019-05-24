using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarthCtrl : MonoBehaviour
{
    // Start is called before the first frame update    
    [SerializeField]
    private float maxHp;//最大HP
    public float hp;//現在のHP
    [SerializeField]
    private int safeSize;//隕石にならないサイズ
    [SerializeField]
    private int defaultScore;//基礎加点数
    private int score;//スコア
    private int pulsScore;
    public bool isDead;//死亡判定

    [SerializeField]
    private PlayerCtrl player;
    public int exp;//追加される経験値
    private int pulsExp;

    public ShootingStar star;//地上視点
    public GameObject earthCamera;//地上カメラ
    private bool isDisplay;//地上カメラが表示されているか
    public float earthTimer = 120f;//地上カメラを表示する時間
    public Number scoreNumber;//スコア描写
    public GameObject over;
    private float timer;
   
    void Start()
    {
        //HP
        hp = maxHp;
        timer = earthTimer;
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (pulsScore != 0)
        {
            score++;
            pulsScore--;
            scoreNumber.Set(score);//スコアを更新
        }

        if (pulsExp != 0)
        {
            pulsExp--;
            player.AddExp(1);
        }
        DisPlayCamera();

        if (hp <= 0)//HP0以下の時
        {
            isDead = true;
            Destroy();
        }

    }


    void Destroy()
    {
        //破壊処理
        Destroy(gameObject);
        over.SetActive(true);
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void AddMeteo(int meteoSize)
    {
        if (meteoSize <= safeSize)
        {
            isDisplay = true;//カメラ表示
            timer = earthTimer;//表示時間をリセット
            score += meteoSize * defaultScore;
            scoreNumber.Set(score);//スコアを更新
            pulsExp = exp;
            star.FallStar();
        }
        else
        {
            //ダメージ
            isDisplay = true;//カメラ表示
            timer = earthTimer;//表示時間をリセット
            hp -= meteoSize;
            star.FallMeteo();
        }
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
