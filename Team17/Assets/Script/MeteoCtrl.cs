﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoCtrl : MonoBehaviour
{
    public float size;//隕石の大きさ
    public float safeSize = 3;//地球にダメージを与えない最大の大きさ
    public float divisionNum = 2;//分裂数
    public float damage = 1;//基礎ダメージ
    [SerializeField]
    private PlayerCtrl player;
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private float speed;


    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (player.GetCatch()) return;
        transform.position = Vector2.Lerp(transform.position, target.transform.position, speed*0.01f);
    }

    //スケールとサイズを分裂数分割る
    void Division()
    {
       transform.localScale *= 1/divisionNum;
        size = size / divisionNum;
    }

    private Vector2 CircleHorizon(float radius,Vector2 colPos)
    {
        var angle = Random.Range(0, 360);
        var rad = angle * Mathf.Deg2Rad;
        var px = Mathf.Cos(rad) * radius + colPos.x;
        var py = Mathf.Sin(rad) * radius + colPos.y;
        return new Vector2(px, py);
    }
  

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Earth")
        {
            EarthCtrl earth = col.gameObject.GetComponent<EarthCtrl>();
            //サイズが一定以下なら加点
            if (size <= safeSize)
             earth.AddScore(size); 
            //一定以上ならダメージ
            else
                earth.Damage(size * damage);
            Destroy(gameObject);
        }

        if(col.gameObject.tag == "Meteo" && player.GetShot())
        {
            Vector2 hitPos;
            Transform spawnpos = transform;
            Division();
            foreach (ContactPoint2D point in col.contacts)
            {
                hitPos = point.point;
               
            }
        }
    }
}
