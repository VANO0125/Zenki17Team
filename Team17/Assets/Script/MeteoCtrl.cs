using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoCtrl : MonoBehaviour
{
    public float size;//隕石の大きさ
    public float safeSize = 5;//地球にダメージを与えない最大の大きさ
    public float divisionNum = 2;//分裂数
    public float damage = 1;//基礎ダメージ
    public int point = 100;//基礎加点スコア
    [SerializeField]
    private PlayerCtrl player;
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private float speed;
    Rigidbody2D rb;
    Vector2[] directions ={
           new Vector2(1,1),
           new Vector2(-1,1),
           new Vector2(1,-1),
           new Vector2(-1,-1)
                                };
    public bool isShot;
    public bool isCaught;
    public int hitNum;
    SpriteRenderer render;

    // Start is called before the first frame update
    void Start()
    {
        //メテオキャッチ
        //transform.localScale *= size;
        isCaught = false;
        hitNum = 0;
        isShot = false;
       // size = Random.Range(3, 15);
        transform.localScale *= 1 + 0.1f * size;
        player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        rb = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ColorChange();
        Move();
        Death();

    }

    void Move()
    {
        rb.AddForceAtPosition((
                               target.transform.position - transform.position).normalized * speed * 0.01f,
                               target.transform.position,
                               ForceMode2D.Impulse
                               );
    }

    //スケールとサイズを分裂数分割る
    void Division(GameObject obj)
    {

        obj.GetComponent<MeteoCtrl>().size = obj.GetComponent<MeteoCtrl>().size / divisionNum;
        obj.transform.localScale *= 1 / divisionNum;
        for (int i = 0; i < divisionNum; i++)
        {
            var divisionMeteo = Instantiate(obj) as GameObject;
            //divisionMeteo.transform.localScale *= 1 / divisionNum;
            //divisionMeteo.GetComponent<MeteoCtrl>().size = obj.GetComponent<MeteoCtrl>().size;
            //divisionMeteo.transform.localScale = obj.transform.localScale;
            divisionMeteo.transform.position = obj.transform.TransformPoint(directions[i] / 4);
        }
    }


    private Vector2 CircleHorizon(float radius, Vector2 colPos)
    {
        var angle = Random.Range(0, 360);
        var rad = angle * Mathf.Deg2Rad;
        var px = Mathf.Cos(rad) * radius + colPos.x;
        var py = Mathf.Sin(rad) * radius + colPos.y;
        return new Vector2(px, py);
    }

    void Death()
    {
        if (size < 1)
        {
            Destroy(gameObject);
        }
    }

    void ColorChange()
    {
        if (size > safeSize)
        {
            render.color = Color.red;
        }
        else
        {
            render.color = Color.blue;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Earth")
        {
            EarthCtrl earth = col.gameObject.GetComponent<EarthCtrl>();
            //サイズが一定以下なら加点
            if (size <= safeSize)
                earth.AddScore((int)size * point);
            //一定以上ならダメージ
            else
                earth.Damage(size * damage);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Meteo" && isShot)
        {
            isShot = false;
            //MeteoCtrl divisionMeteo = col.gameObject.GetComponent<MeteoCtrl>();
            Division(col.gameObject);
        }
    }
}