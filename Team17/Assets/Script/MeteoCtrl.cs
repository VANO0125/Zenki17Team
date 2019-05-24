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
    public MeteoCtrl[] meteos;
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
    private Transform playerPos;
    private Rigidbody2D playerRig;

    private float timer;
    public float maxHp;
    public float hp;
    TrailRenderer shotEffect;
    public AudioClip Sebreak;
    public AudioClip Seattck;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        hp = maxHp;
        rig = GetComponent<Rigidbody2D>();

        //子オブジェクトがあればサイズを合計
        if (isParent)
        {
            meteos = new MeteoCtrl[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                meteos[i] = transform.GetChild(i).GetComponent<MeteoCtrl>();
                if (meteos[i].isParent)
                {
                    MeteoCtrl m = meteos[i];
                    MeteoCtrl[] ms = new MeteoCtrl[m.transform.childCount];
                    m.parent = this;
                    for (int j = 0; j < m.transform.childCount; j++)
                    {
                        ms[j] = m.transform.GetChild(j).GetComponent<MeteoCtrl>();
                        size += ms[j].size;
                        hp += ms[j].hp;
                    }
                }
                else
                {
                    meteos[i].parent = this;
                    size += meteos[i].size;
                    hp += meteos[i].hp;
                }
            }
        }
        else
        {
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
        //if (transform.childCount == 1)
        //{
        //    var child = transform.GetChild(0).gameObject;
        //    child.layer = 8;
        //    child.GetComponent<Rigidbody2D>().isKinematic = false;
        //    child.transform.parent = null;
        //    Destroy(gameObject);
        //}
        Move();
        Death();
        ShotEffect();

        if (isShot)
            timer++;
        else
        {
            if (earth != null)
                target = earth.transform;
            timer = 0;
        }

        if (timer >= 180)
        {
            isShot = false;
            rig.velocity = Vector2.zero;
            //rig.isKinematic = false;
        }

        if (!isCaught)
            rig.mass = 10;
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
            rig.mass = playerRig.mass;
            rig.velocity = playerRig.velocity;
        }
        else if (target != null)
            rig.velocity = (target.position - transform.position).normalized * speed;
    }

    IEnumerator ChangeLayer(GameObject meteo)
    {
        meteo.layer = 11;
        yield return new WaitForSeconds(0.3f);
        meteo.layer = 8;
    }

    void DivisionAll(Transform core)
    {
        //隕石を分離させる
        for (int i = 0; i < meteos.Length; i++)
        {
            if (isParent && meteos[i] != null)
            {
                //StartCoroutine(ChangeLayer(meteos[i].gameObject));

                meteos[i].isShot = true;
                meteos[i].SetKinematic(false);
                meteos[i].rig.AddForce((meteos[i].transform.position - core.position).normalized * 300, ForceMode2D.Impulse);
                meteos[i].hp = 0;
            }
            audioSource.PlayOneShot(Sebreak);
        }
    }

    public void SetTarget(Transform target)
    { this.target = target; }

    //プレイヤーに掴まれる処理
    public void Caught(Transform parent, Rigidbody2D rig)
    {
        foreach (var m in GetHighest().GetAll())
        {
            m.isShot = false;
            m.playerPos = parent;
            m.playerRig = rig;
            m.SetSimulated(false);
        }
            isCaught = true;
            transform.parent = parent;
    }

    //隕石射出処理
    public void ShotMeteo(Vector2 vec, float shotPower, float power, Transform player)
    {
        foreach (var m in GetHighest().GetAll())
        {
            m.rig.simulated = true;
            m.isCaught = false;
            m.shotVec = vec;
            m.shotPower = shotPower;
            m.power = power;
            m.isShot = true;
            m.playerPos = player;
        }
            transform.parent = null;
            rig.AddForce(vec * shotPower, ForceMode2D.Impulse);
    }

    void ShotEffect()
    {
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
            if (isCore)
            {
                GetUnitMeteo().DivisionAll(transform);
                isCore = false;
            }
            //  GetUnitMeteo().hp -= maxHp;

            transform.parent = null;
            parent = null;
            isDead = true;
            SetKinematic(false);
            audioSource.PlayOneShot(Sebreak);
            DamageEffect(transform.position);
        }
    }

    //分裂しきっていない時のダメージ処理
    public void TotalAddMeteo(EarthCtrl earth)
    {
        //earth.AddMeteo(size);
        //Destroy(gameObject);
    }

    public MeteoCtrl GetParent()
    { return parent; }

    public MeteoCtrl GetHighest()
    {
        var p = parent;
        var hightest = this;
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
        if (no2 == null)
            return this;
        else
            return no2;
    }

    public List<MeteoCtrl> GetAll()
    {
        List<MeteoCtrl> ms = new List<MeteoCtrl>();
        if (!isParent) return null;
        else
        {
            ms.Add(this);
            for (int i = 0; i < meteos.Length; i++)
            {
                if (meteos[i] != null)
                {
                    if (meteos[i].isParent)
                    {
                        MeteoCtrl m = meteos[i];
                        for (int j = 0; j < m.transform.childCount; j++)
                            ms.Add(m.transform.GetChild(j).GetComponent<MeteoCtrl>());
                    }
                }
                    ms.Add(meteos[i]);
            }
            return ms;
        }
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
            EarthCtrl earth = col.gameObject.GetComponent<EarthCtrl>();
           earth.AddMeteo(GetHighest().size);
            Destroy(GetHighest().gameObject);
            //Debug.Log("nu"+earth.hp);
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
}