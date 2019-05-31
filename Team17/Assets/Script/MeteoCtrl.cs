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
    private Vector3 localPos;

    public Transform target;
    public EarthCtrl earth;//地球

    [SerializeField]
    private GameObject damageEffect;//パンチされたときのエフェクト
    [SerializeField]
    private GameObject partile;
    GameObject meteoParticle;

    public bool isShot;
    private bool isCaught;
    public bool isCore, isParent;

    private float shotPower;
    private Vector2 shotVec;
    private Transform playerPos;
    private Rigidbody2D playerRig;
    [SerializeField]
    private GameObject shotEffect;
    private GameObject shotObject;

    private float shotTime;
    private float timer;
    public float maxHp;
    [SerializeField]
    private bool maxParent;
    public float hp;
    private int layerNum;
    private List<int> saveNum;
    public AudioClip Sebreak;
    public AudioClip Seattck;
    AudioSource audioSource;


    // Start is called before the first frame update
    void Awake()
    {
        shotTime = 180;
        localPos = transform.localPosition;

        hp = maxHp;
        rig = GetComponent<Rigidbody2D>();

        //子オブジェクトがあればサイズを合計
        if (isParent )
        {
            earth = GameObject.FindGameObjectWithTag("Earth").GetComponent<EarthCtrl>();
            if(!GetHighest().gameObject.GetComponent<MeteoCtrl>().maxParent)
            {
                layerNum = MeteoLayer.Instance.GetLayer();
            }
            else
            {
                layerNum = GetHighest().gameObject.GetComponent<MeteoCtrl>().layerNum;
            }          
            gameObject.layer = layerNum;
            maxParent = true;
            meteos = new MeteoCtrl[transform.childCount];
            earth = GameObject.FindGameObjectWithTag("Earth").GetComponent<EarthCtrl>();
            for (int i = 0; i < transform.childCount; i++)
            {

                meteos[i] = transform.GetChild(i).GetComponent<MeteoCtrl>(); ;
                meteos[i].gameObject.layer = layerNum;
                if (meteos[i].isParent)
                {
                    MeteoCtrl m = meteos[i];
                    MeteoCtrl[] ms = new MeteoCtrl[m.transform.childCount];
                    m.parent = this;
                    for (int j = 0; j < m.transform.childCount; j++)
                    {
                        ms[j] = m.transform.GetChild(j).GetComponent<MeteoCtrl>();
                        ms[j].gameObject.layer = layerNum;
                        size += ms[j].size;
                        hp += ms[j].maxHp;
                    }
                }
                else
                {
                    meteos[i].parent = this;
                    size += meteos[i].size;
                    hp += meteos[i].maxHp;
                }
            }
            maxHp = hp;
        }
        else
        {
            earth = parent.earth;
            speed = parent.speed;
        }
        audioSource = GetComponent<AudioSource>();
        if (!isParent)
        {
            meteoParticle = Instantiate(partile, transform.position, Quaternion.identity);
            meteoParticle.transform.parent = transform;
            meteoParticle.SetActive(false);
        }
        target = earth.transform;
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.isStart)
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

            if (size > earth.safeSize)
                shotTime = 180 / size <= 30 ? 180 / size : 30;
            else shotTime = 180;
            if (isShot)
            {
                timer++;
                if (timer >= GetHighest().shotTime)
                {
                    isShot = false;
                    rig.velocity = Vector2.zero;
                    rig.isKinematic = false;
                }
            }
            else
            {
                if (earth != null)
                    target = earth.transform;
                timer = 0;
            }


            if (!isShot && shotObject != null)
                Destroy(shotObject);

            ReMove();
            if (GetHighest().size <= earth.safeSize && meteoParticle != null)
                meteoParticle.SetActive(true);
            else if (meteoParticle != null)
                meteoParticle.SetActive(false);
            if (transform.parent != null && transform.parent.gameObject.tag == "Meteo")
            {
                transform.localPosition = localPos;
                rig.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else rig.constraints = RigidbodyConstraints2D.None;
            if (transform.childCount == 1 && transform.GetChild(0).tag == "Meteo")
            {
                transform.GetChild(0).parent = null;
                MeteoLayer.Instance.ChangeBool(layerNum);
                Destroy(gameObject);
            }
            if (!isCaught)
                rig.mass = 10;
            ReMove();

            if (GetHighest().size <= earth.safeSize && meteoParticle != null)
                meteoParticle.SetActive(true);
            else if (meteoParticle != null)
                meteoParticle.SetActive(false);
            if (transform.parent != null && transform.parent.gameObject.tag == "Meteo")
            {
                transform.localPosition = localPos;
                rig.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else rig.constraints = RigidbodyConstraints2D.None;
            if (transform.childCount == 1 && transform.GetChild(0).tag == "Meteo")
            {
                transform.GetChild(0).parent = null;
                DestroyNum();
                Destroy(gameObject);
            }
        }
    }

    public void SetSize(int size)
    {
        this.size = size;
    }

    void Move()
    {
        if (transform.parent != null && transform.parent.gameObject.tag == "Meteo")
        {
            transform.localPosition = localPos;
            rig.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else if (!isShot) rig.constraints = RigidbodyConstraints2D.None;
        if (isShot)
        {
            rig.constraints = RigidbodyConstraints2D.FreezeRotation;
            return;
        }
        //  else if (isCaught)
        {
            //  rig.mass = playerRig.mass;
            //   rig.velocity = playerRig.velocity;
        }

        if (target != null && transform.parent == null)
        {
            foreach (var m in GetAll())
            {
                if (m != null)
                {
                    //float gravity = (target.position - transform.position).magnitude<50? 20:1;

                    // m.rig.velocity = (target.position - transform.position).normalized * speed * accel;
                    //  if(rig.velocity.magnitude<=speed)
                    if (size <= earth.safeSize)
                        m.rig.AddForce((target.position - transform.position).normalized * speed);
                    else
                        m.rig.velocity = ((target.position - transform.position).normalized * speed);
                }
            }
        }
    }

    void ChangeLayer(MeteoCtrl meteo, int num)
    {
        meteo.gameObject.layer = num;
    }

    void ReMove()
    {
        if (isParent)
        {
            for (int i = 0; i < meteos.Length; i++)
            {
                if (meteos[i] != null && meteos[i].isDead)
                    meteos[i] = null;
            }
            if (transform.childCount == 0)
            {
                if (layerNum != 8)
                    MeteoLayer.Instance.ChangeBool(layerNum);
                if (shotObject != null)
                    Destroy(shotObject);
                Destroy(gameObject);
            }
                DestroyNum();
                Destroy(gameObject);
            }
        
        if (transform.childCount == 1 && transform.GetChild(0).tag == "Meteo")
        {
            transform.GetChild(0).parent = null;
            if (layerNum != 8)
                MeteoLayer.Instance.ChangeBool(layerNum);
            Destroy(gameObject);
        }
    }

    void DestroyNum()
    {
        if (layerNum != 8)
            MeteoLayer.Instance.ChangeBool(layerNum);
    }

    void DivisionAll(Transform core)
    {
        //隕石を分離させる
        for (int i = 0; i < meteos.Length; i++)
        {
            if (isParent && meteos[i] != null)
            {
                meteos[i].isShot = true;
                meteos[i].gameObject.layer = 8;
                meteos[i].SetKinematic(false);
                meteos[i].rig.AddForce((meteos[i].transform.position - core.position).normalized * 3, ForceMode2D.Impulse);
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
        transform.parent = parent;

        foreach (var m in GetAll())
        {
            if (m != null)
            {
                m.isShot = false;
                m.isCaught = true;
                //   m.playerPos = parent;
                m.rig.velocity = Vector2.zero;
                m.SetSimulated(false);
            }
        }
        isCaught = true;
        transform.parent = parent;
    }

    //隕石射出処理
    public void ShotMeteo(Vector2 vec, float shotPower, float power, Transform player)
    {
        transform.parent = null;

        if (shotObject == null)
            shotObject = Instantiate(shotEffect, transform.position, Quaternion.identity);
        shotObject.transform.parent = transform;
        foreach (var m in GetAll())
        {
            if (m != null)
            {
                m.rig.simulated = true;
                m.power = power;
                m.isShot = true;
                m.isCaught = false;
                m.shotVec = vec;
                m.shotPower = shotPower;
                m.power = power;
                m.playerPos = player;
                if (size <= earth.safeSize)
                    m.rig.AddForce(vec * shotPower, ForceMode2D.Impulse);
                else
                    m.rig.AddForce(vec * shotPower / size, ForceMode2D.Impulse);
            }
        }
    }

    public void SetKinematic(bool flag)
    {
        rig.isKinematic = flag;
    }

    public void SetSimulated(bool flag)
    {
        rig.simulated = flag;
    }

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
            ChangeLayer(this, 8);
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
        ms.Add(this);
        if (isParent)
        {
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
        }
        return ms;
    }

    public void Damage(float damage)
    {
        hp -= damage;
        MeteoCtrl h = GetHighest();
        MeteoCtrl u = GetUnitMeteo();

        if (h != u)
        {
            h.hp -= damage;
            u.hp -= damage;
        }
        else if (h != this)
            h.hp -= damage;

        if (damage / h.maxHp >= 1)
            h.DivisionAll(transform);
        else if (damage / h.maxHp >= 0.5f)
            u.hp = 0;
    }

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
                if (GetTotalSize() > earth.safeSize)
                {
                    Damage(otherMeteo.power * otherMeteo.GetHighest().size);
                    otherMeteo.hp = 0;
                    foreach (ContactPoint2D point in col.contacts)
                        DamageEffect(point.point);
                    otherMeteo.isShot = false;
                }
                else
                {
                    isShot = true;
                    foreach (ContactPoint2D point in col.contacts)
                    {
                        if (shotObject == null)
                            shotObject = Instantiate(shotEffect, otherMeteo.transform.position, Quaternion.identity);
                        shotObject.transform.parent = transform;
                        //     rig.AddForceAtPosition((transform.position - col.transform.position).normalized * shotPower, point.point);
                    }
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