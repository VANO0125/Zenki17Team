using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeteoCtrl : MonoBehaviour
{
    public int size;//隕石の大きさ
    [SerializeField]
    public Rigidbody2D rig;

    [SerializeField]
    private int number;//子供の番号
    [SerializeField]
    private MeteoCtrl parent;//親オブジェクト
    private MeteoCtrl[] meteos;
    public Transform target;
    public float speed;
    [SerializeField]
    private MeteoCtrl meteo;//死亡時生成する隕石
    [SerializeField]
    private EarthCtrl earth;//地球

    [SerializeField]
    private GameObject effect;//分裂時のエフェクト
    [SerializeField]
    private GameObject damageEffect;//パンチされたときのエフェクト

    [SerializeField]
    private float power;
    public bool isShot;
    private bool isCaught;
    public bool isCore,isParent;
    
    private float shotPower;
    private Vector2 shotVec;
    [SerializeField]
    private Transform playerPos;

    private float timer;
    public float hp;
    TrailRenderer shotEffect;
    public AudioClip Sebreak;
    public AudioClip Seattck;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        //子オブジェクトがあればサイズを合計
        if (isParent)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                meteos = new MeteoCtrl[transform.childCount];
                meteos[i] = transform.GetChild(0).GetComponent<MeteoCtrl>();
                size += meteos[i].size;
            }
        }
        else
        {
            if(transform.parent != null)
            parent = transform.parent.GetComponent<MeteoCtrl>();
        }
        shotEffect = GetComponent<TrailRenderer>();
        audioSource = GetComponent<AudioSource>();
        shotEffect.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (transform.childCount == 1)
        //{
        //    var child = transform.GetChild(0).gameObject;
        //    child.layer = 8;
        //    child.GetComponent<Rigidbody2D>().isKinematic = false;
        //    child.transform.parent = null;
        //    Destroy(gameObject);
        //}
        //ColorChange();
        Move();
        Death();
        ShotEffect();
        if (isShot)
            timer++;
        else
            timer = 0;

        if (timer >= 180)
        {
            isShot = false;
            //rig.isKinematic = false;
        }
    }

    public void SetSize(int size)
    {
        this.size = size;
    }

    void Move()
    {
        if (isShot) return;
        else if (isCaught)
            transform.position = Vector3.MoveTowards(transform.position, playerPos.position, 30 * Time.deltaTime);
        else if (target != null && parent == null && size != 0) //rig.velocity =((target.position-transform.position).normalized*speed);
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed *  Time.deltaTime);
    }

    void Division(int number)
    {
        if (meteos[number] == null) return;
        //隕石を分離させる
        //meteos[number].transform.parent = null;
        //meteos[number].parent = null;
        //meteos[number].SetKinematic(false);
        //meteos[number].rig.AddForce((meteos[number].transform.position - transform.position).normalized * 10, ForceMode2D.Impulse);
        //meteos[number] = null;
        //audioSource.PlayOneShot(Sebreak);
    }

    void DivisionAll()
    {
        if (parent != null)
        {
            transform.parent = null;
            parent = null;
            SetKinematic(false);
        }
        else
        {
            //隕石を分離させる
            for (int i = 0; i < meteos.Length; i++)
            {
                if (meteos[i] != null)
                    meteos[i].hp = 0;
                audioSource.PlayOneShot(Sebreak);
            }
        }
    }

    public void SetTarget(Transform target)
    { this.target = target; }

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
    public void ShotMeteo(Vector2 vec,float shotPower, float power, Transform player)
    {
        rig.simulated = true;
        transform.parent = null;
        shotVec = vec;
        this.shotPower = shotPower;
        this.power = power;
        isShot = true;
        playerPos = player;
        rig.AddForce(vec * shotPower, ForceMode2D.Impulse);
    }

    void ShotEffect()
    {
        if (number != 0) return;
        if (isShot)
        {
            shotEffect.enabled = true;
        }
        else
        {
            shotEffect.enabled = false;
        }
    }

    public void SetKinematic(bool flag)
    { rig.isKinematic = flag; }

    public void SetSimulated(bool flag)
    { rig.simulated = flag; }

    void Death()
    {
        if (isParent)
        {
            for (int i = 0; i < meteos.Length; i++)
            {
                if (meteos[i] != null)
                    hp += meteos[i].hp;
            }
        }
        if (hp <= 0)
        {
            hp = 0;
            if (isCore)
            {
                GetUnitMeteo().DivisionAll();
                isCore = false;
            }
            if (!isParent)
            {
                hp = 20;
                transform.parent = null;
                parent = null;
                SetKinematic(false);      
                audioSource.PlayOneShot(Sebreak);
            }
        }
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
        var t = parent;
        var no2 = parent;
        int stoper = 0;

        while (hightest != null)
        {
            t = hightest;
            hightest = hightest.GetParent();
            if (hightest != null)
                no2 = t;
            stoper++;
            if (stoper >= 300)
                break;
        }
        return no2;
    }

    public void Damage(float damage)
    {
        hp -= damage;
        //  DamageEffect();
    }

    public void DamageEffect(Vector2 position)
    {       
        var obj = Instantiate(damageEffect, position, Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Earth")
        {
          //  EarthCtrl earth = col.gameObject.GetComponent<EarthCtrl>();
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
                if (GetTotalSize() > 1)
                {
                    Damage(otherMeteo.power);
                    otherMeteo.hp = 0;
                    otherMeteo.isShot = false;
                    foreach (ContactPoint2D point in col.contacts)
                        DamageEffect(point.point);
                }
                else
                {
                    isShot = true;
                    foreach (ContactPoint2D point in col.contacts)
                        rig.AddForceAtPosition((transform.position-col.transform.position).normalized*shotPower,point.point);
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        //if (col.gameObject.tag == "Catcher" && transform.parent == null)
        //{
        //    isCaught = true;
        //}
    }

    void OnTriggerExit2D(Collider2D col)
    {
        //if (col.gameObject.tag == "Catcher")
        //    isCaught = false;
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