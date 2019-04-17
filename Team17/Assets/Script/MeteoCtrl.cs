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
    public bool isShot;
    public bool isCaught;

    private float timer;

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
        MeteoCtrl newMeteo = Instantiate(meteos[number], transform.position, Quaternion.identity) as MeteoCtrl;
        newMeteo.ChangeRig();
        Destroy(meteos[number].gameObject);
    }

    public void ChangeRig()
    { rig.isKinematic = false; }

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
                parent.TotalAddMeteo(earth);
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
                //MeteoCtrl otherMeteo = ;
                parent.Division(number);
                otherMeteo.isShot = false;
            }
        }

        if (col.gameObject.tag == "stage")
        {
            //Destroy(gameObject);
        }

    }
}