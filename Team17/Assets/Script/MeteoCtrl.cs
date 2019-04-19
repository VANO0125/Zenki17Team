using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoCtrl : MonoBehaviour
{
    public int size;//隕石の大きさ
    [SerializeField]
    public Rigidbody2D rig;

    [SerializeField]
    private int number;//子供の番号
    [SerializeField]
    private MeteoCtrl parent;//親オブジェクト
    [SerializeField]
    private MeteoCtrl[] meteos;
    private GameObject target;
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject color;//色を変える部分
    [SerializeField]
    private GameObject effect;//分裂時のエフェクト
    private bool isShot;
    private bool isCaught;

    private float timer;
    public int hp;

    // Start is called before the first frame update
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Earth");
        //子オブジェクトがあればサイズを合計
        if (parent == null)
        {
            for (int i = 0; i < meteos.Length; i++)
            {
                size += meteos[i].size;
            }
        }

        //メテオキャッチ
        isCaught = false;
        isShot = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 1)
        {
            var child = transform.GetChild(0).gameObject;
            child.layer = 8;
            child.GetComponent<Rigidbody2D>().isKinematic = false;
            child.transform.parent = null;
            Destroy(gameObject);
        }
        //ColorChange();
        Move();
        Death();
        if (isShot)
            timer++;
        if (timer >= 180)
        {
            isShot = false;
            timer = 0;
        }

        if (hp <= 0)
        {
            //   Division();
        }
    }

    public void SetSize(int size)
    {
        this.size = size;
    }

    void Move()
    {
        if (isCaught || isShot) return;
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
    }

    void Division(int number)
    {
        if (meteos[number] == null) return;
        //隕石を分離させる
        meteos[number].gameObject.layer = 8;
        meteos[number].transform.parent = null;
        meteos[number].parent = null;
        //MeteoCtrl newMeteo = Instantiate(meteos[number], transform.position, Quaternion.identity) as MeteoCtrl;
        meteos[number].SetKinematic(false);
        meteos[number] = null;
        //Destroy(meteos[number].gameObject);
    }

    //プレイヤーに掴まれる処理
    public void Caught(Transform parent)
    {
        rig.simulated = false;
        isCaught = true;
        transform.parent = parent;
    }

    //隕石射出処理
    public void ShotMeteo(Vector2 vec, float power)
    {
        rig.simulated = true;
        isShot = true;
        isCaught = false;
        transform.parent = null;
        rig.AddForce(vec * power, ForceMode2D.Impulse);
    }

    public void SetKinematic(bool flag)
    { rig.isKinematic = flag; }

    public void SetSimulated(bool flag)
    { rig.simulated = flag; }

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

    //分裂しきっていない時のダメージ処理
    public void TotalAddMeteo(EarthCtrl earth)
    {
        earth.AddMeteo(size);
        Destroy(gameObject);
    }

    public MeteoCtrl GetParent()
    { return parent; }

    public MeteoCtrl GetHighest()
    {
        var p = parent;
        MeteoCtrl hightest = null;
        int stoper = 0;
        while (p != null)
        {
            if (p != null)
                hightest = p;
            p = p.GetParent();
            stoper++;
            if (stoper >= 300)
                break;
        }
        return hightest;
    }

    public int GetTotalSize()
    {
        var hightest = parent;
        var totalSize = size;
        int stoper = 0;
        while (hightest != null)
        {
            hightest = hightest.GetParent();
            if (hightest != null)
                totalSize = hightest.size;
            stoper++;
            if (stoper >= 300)
                break;
        }
        return totalSize;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Earth")
        {
            EarthCtrl earth = col.gameObject.GetComponent<EarthCtrl>();
            if (parent == null)
            {
                earth.AddMeteo(size);
                Destroy(gameObject);
            }
            else
                GetHighest().TotalAddMeteo(earth);
            //サイズが一定以下なら加点

        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Meteo" && parent != null)
        {
            var otherMeteo = col.gameObject.GetComponent<MeteoCtrl>();
            if (otherMeteo.isShot)
            {
                var hightest = parent;
                var no2 = parent;
                var hMeteo = parent;
                var divNum = number;
                int stoper = 0;
                while (hightest != null)
                {
                    no2 = hightest;
                    hightest = hightest.GetParent();
                    if (hightest != null)
                    {
                        hMeteo = hightest;
                        divNum = no2.number;
                    }
                    stoper++;
                    if (stoper >= 300)
                        break;
                }
                hMeteo.Division(divNum);
                otherMeteo.isShot = false;
            }
        }

        if (col.gameObject.tag == "stage")
        {
            //Destroy(gameObject);
        }


    }
}