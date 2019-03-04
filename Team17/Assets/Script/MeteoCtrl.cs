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
    private Vector3 startScale;//基礎サイズ
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject color;//色を変える部分
    [SerializeField]
    private GameObject effect;//分裂時のエフェクト
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

    // Start is called before the first frame update
    void Awake()
    {
        //メテオキャッチ
        isCaught = false;
        hitNum = 0;
        isShot = false;
        rb = GetComponent<Rigidbody2D>();
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        ColorChange();
        Move();
        Death();
    }

    public void SetSize(float size)
    {
        this.size = size;
        transform.localScale = startScale * size;
    }

    void Move()
    {
        rb.AddForce((target.transform.position - transform.position).normalized * speed);
    }

    //スケールとサイズを分裂数分割る
    void Division(GameObject obj)
    {
        Instantiate(effect, transform.position, Quaternion.identity);
        obj.GetComponent<MeteoCtrl>().SetSize(obj.GetComponent<MeteoCtrl>().size / divisionNum);
        //obj.transform.localScale *= 1 / divisionNum;
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
        if (size < 0.25)
        {
            Destroy(gameObject, 2f);
        }
    }

    void ColorChange()
    {
        if (size > safeSize)
            color.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
        else if (size < 0.25)
            color.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        else
            color.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.5f);
        
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
            Division(col.gameObject);
            Destroy(col.gameObject);
        }
    }
}