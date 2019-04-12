using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoCtrl : MonoBehaviour
{
    private float totalSize;
    public float size;//隕石の大きさ
    public float safeSize = 5;//地球にダメージを与えない最大の大きさ
    public float divisionNum = 2;//分裂数
    public float damage = 1;//基礎ダメージ
    public int point = 100;//基礎加点スコア
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
<<<<<<< HEAD
    public bool isHit;
    public int hitNum;
=======
>>>>>>> 3e5e8bb707ee0fa3c604175252096a16de7f8fa5

    // Start is called before the first frame update
    void Awake()
    {
        //子オブジェクトがあればサイズを合計
        if (transform.childCount == 0)
            totalSize = size;
        else
        {
            for (int i = 0; i < transform.childCount; i++)
                totalSize += transform.GetChild(i).GetComponent<MeteoCtrl>().size;
        }

        //メテオキャッチ
        isCaught = false;
        isShot = false;
        isHit = false;
        rb = GetComponent<Rigidbody2D>();
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
    }

    void Move()
    {
<<<<<<< HEAD
        if (isCaught) return;
        if(target.transform.position != transform.position)
        {
            transform.position = Vector3.MoveTowards(this.gameObject.transform.position, target.transform.position, speed * Time.deltaTime);
        }
=======
         rb.AddForce((target.transform.position - transform.position).normalized * speed);
>>>>>>> 3e5e8bb707ee0fa3c604175252096a16de7f8fa5
    }

    //スケールとサイズを分裂数分割る
    void Division(int num)
    {
<<<<<<< HEAD
        Instantiate(effect, transform.position, Quaternion.identity);
        obj.GetComponent<MeteoCtrl>().SetSize(obj.GetComponent<MeteoCtrl>().size / divisionNum);
        for (int i = 0; i < divisionNum; i++)
        {
            var divisionMeteo = Instantiate(obj) as GameObject;
            divisionMeteo.transform.position = obj.transform.TransformPoint(directions[i] / 4);
=======
        //隕石をnum分分離させる
        for (int i = 0; i < num; i++)
        {
            totalSize -= transform.GetChild(i).GetComponent<MeteoCtrl>().size;
            transform.GetChild(i).gameObject.layer = 8;
            transform.GetChild(i).GetComponent<Rigidbody2D>().isKinematic = false;
            transform.GetChild(i).GetComponent<CircleCollider2D>().isTrigger = false;
            transform.GetChild(i).parent = null;
>>>>>>> 3e5e8bb707ee0fa3c604175252096a16de7f8fa5
        }
    }

    void Death()
    {
        //if (size < 0.25)
        //{
        //    Destroy(gameObject, 2f);
        //}
    }

    void ColorChange()
    {
        //if (size > safeSize)
        //    color.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
        //else if (size < 0.25)
        //    color.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        //else
        //    color.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.5f);

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Earth")
        {
            isHit = true;
            EarthCtrl earth = col.gameObject.GetComponent<EarthCtrl>();
            //サイズが一定以下なら加点
            if (totalSize <= safeSize)
                earth.AddScore((int)totalSize * point);
            //一定以上ならダメージ
            else
                earth.Damage(totalSize * damage);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Meteo" && col.gameObject.GetComponent<MeteoCtrl>().isShot && transform.childCount > 0)
        {
<<<<<<< HEAD
            isHit = true;
            isShot = false;
            Division(col.gameObject);
            Destroy(col.gameObject);
=======
            col.gameObject.GetComponent<MeteoCtrl>().isShot = false;
            Division(1);
>>>>>>> 3e5e8bb707ee0fa3c604175252096a16de7f8fa5
        }
        if(col.gameObject.tag == "stage")
        {
            Destroy(gameObject);
        }
    }
}