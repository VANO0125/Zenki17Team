using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeteoCtrl : MonoBehaviour
{
    public int size;//隕石の大きさ
    public Rigidbody2D rig;
    private bool isDead;

    [SerializeField]
    private int number;//子供の番号
    [SerializeField]
    private MeteoCtrl parent;//親オブジェクト
    private MeteoCtrl[] meteos;
    public float speed;
    [SerializeField]
    private float power;

    public Transform target;
    public EarthCtrl earth;//地球

    [SerializeField]
    private GameObject effect;//分裂時のエフェクト
    [SerializeField]
    private GameObject damageEffect;//パンチされたときのエフェクト

    public bool isShot;
    private bool isCaught;
    public bool isCore, isParent;

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
    [SerializeField]
    private int meteoID = 0;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        //子オブジェクトがあればサイズを合計
        if (isParent)
        {
            meteos = new MeteoCtrl[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                meteos[i] = transform.GetChild(i).GetComponent<MeteoCtrl>();
                size += meteos[i].size;
            }
        }
        else
        {
            parent = transform.parent.GetComponent<MeteoCtrl>();
            earth = parent.earth;
            speed = parent.speed;
        }

        shotEffect = GetComponent<TrailRenderer>();
        audioSource = GetComponent<AudioSource>();
        shotEffect.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
 
        Move();
        Death();
        ShotEffect();
        if (isShot)
            timer++;
        else
        {
            target = earth.transform;
            timer = 0;
        }

        if (timer >= 180)
        {
            isShot = false;
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
        {
            return;
        }
        else if (target != null && parent == null && size != 0)
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
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
        //隕石を分離させる
        for (int i = 0; i < meteos.Length; i++)
        {
            if (isParent)
            {
                meteos[i].hp = 0;
            }
                
        }
        audioSource.PlayOneShot(Sebreak);
    }

    public void SetTarget(Transform target)
    { this.target = target; }

    //プレイヤーに掴まれる処理
    public void Caught(GameObject parent,Vector3 catchPos)
    {
        isCaught = true;
        isShot = false;
        transform.parent = parent.transform;
        SetSimulated(false);
        
    }

    //隕石射出処理
    public void ShotMeteo(Vector2 vec, float shotPower, float power, Transform player)
    {
        rig.simulated = true;
        transform.parent = null;
        isCaught = false;
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
        if (hp <= 0 && !isDead)
        {
            if (isParent)
            {
               
            }
            else
            {
                if (isCore)
                {
                    GetUnitMeteo().DivisionAll();
                    isCore = false;
                }
                transform.parent = null;
                parent = null;
            }
            hp = 0;
            isDead = true;
            SetKinematic(false);
            audioSource.PlayOneShot(Sebreak);
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
    { hp -= damage; }

    public void DamageEffect(Vector2 position)
    {
        var obj = Instantiate(damageEffect, position, Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Earth")
        {
            if (!isParent)
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
                    foreach (ContactPoint2D point in col.contacts)
                        DamageEffect(point.point);
                    otherMeteo.isShot = false;
                }
                else
                {
                    isShot = true;
                    foreach (ContactPoint2D point in col.contacts)
                        rig.AddForceAtPosition((transform.position - col.transform.position).normalized * shotPower, point.point);
                }
            }
        }
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

    public int GetMeteoID()
    {
        return meteoID;
    }
}