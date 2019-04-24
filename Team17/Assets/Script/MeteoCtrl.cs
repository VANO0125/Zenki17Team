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
    public Transform target;
    public float speed;
    [SerializeField]
    private GameObject color;//色を変える部分
    [SerializeField]
    private GameObject effect;//分裂時のエフェクト
    public bool isShot;
    private bool isCaught;
    public bool isCore;

    private Vector2 shotVec;
    private Transform playerPos;
    private bool isRefect;
    private GameObject shotEffect;

    private float timer, rTimer;
    public int hp;

    // Start is called before the first frame update
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Earth").transform;
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
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
        else
            timer = 0;

        if (isRefect)
        {
            rTimer++;
            if (Mathf.Abs((transform.position - playerPos.position).magnitude) < 0.5)
                isRefect = false;
        }
        else
            rTimer = 0;
        if (timer >= 180)
        {
            isShot = false;
            rig.isKinematic = false;
        }
        if (rTimer >= 180)
        {
            isRefect = false;
            rig.isKinematic = false;
        }
    }

    public void SetSize(int size)
    {
        this.size = size;
    }

    void Move()
    {
        if (isShot)
            transform.position = Vector3.MoveTowards(transform.position, shotVec + (Vector2)transform.position, 50 * Time.deltaTime);
        else if (isCaught)
            rig.AddForce((playerPos.position-transform.position).normalized * 200);
        else if (isRefect)
            transform.position = Vector3.MoveTowards(transform.position, playerPos.position, 50 * Time.deltaTime);
        else if (target != null && parent == null && size != 0)
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed / size * Time.deltaTime);
    }

    void Division(int number)
    {
        if (meteos[number] == null) return;
        //隕石を分離させる
        meteos[number].transform.parent = null;
        meteos[number].parent = null;
        meteos[number].SetKinematic(false);
        meteos[number] = null;
    }

    void DivisionAll()
    {
        //隕石を分離させる
        for (int i = 0; i < meteos.Length; i++)
        {
            if (meteos[i] != null)
            {
                meteos[i].transform.parent = null;
                meteos[i].parent = null;
                meteos[i].SetKinematic(false);
                meteos[i] = null;
            }
        }
        Destroy(gameObject);
    }

    //プレイヤーに掴まれる処理
    public void Caught(Transform parent)
    {
        rig.simulated = false;
        isCaught = true;
        isShot = false;
        transform.parent = parent;
        playerPos = parent;
    }

    //隕石射出処理
    public void ShotMeteo(Vector2 vec, float power, Transform player)
    {
        shotVec = vec;
        //  shotEffect = Instantiate(trail, transform.position, Quaternion.identity) as GameObject;
        //  shotEffect.transform.parent = transform;
        //rig.simulated = true;
        isShot = true;
        playerPos = player;
        //isCaught = false;
        //transform.parent = null;
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
        if (hp <= 0 && parent != null)
            if (isCore)
                GetUnitMeteo().DivisionAll();
            else
                parent.Division(number);
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

    public MeteoCtrl GetUnitMeteo()
    {
        var hightest = parent;
        var no2 = parent;
        int stoper = 0;

        while (hightest != null)
        {
            no2 = hightest;
            hightest = hightest.GetParent();
            stoper++;
            if (stoper >= 300)
                break;
        }
        return no2;
    }

    public void Damage(int damage)
    { hp -= damage; }

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
        if (col.gameObject.tag == "Meteo")
        {
            var otherMeteo = col.gameObject.GetComponent<MeteoCtrl>();
            if (otherMeteo.isShot)
            {
                Damage(5);
                //GetHighest().Division(GetDivNumber());
                otherMeteo.isShot = false;
                otherMeteo.isRefect = true;
            }
        }

        if (col.gameObject.tag == "Player")
        {
            isRefect = false;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Catcher")
        {
            isCaught = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Catcher")
            isCaught = false;
    }

    public int GetTotalSize()
    {
        var hightest = parent;
        var totalSize = size;
        int stoper = 0;
        while (hightest != null)
        {
            if (hightest != null)
                totalSize = hightest.size;
            hightest = hightest.GetParent();
            stoper++;
            if (stoper >= 300)
                break;
        }
        return totalSize;
    }
}