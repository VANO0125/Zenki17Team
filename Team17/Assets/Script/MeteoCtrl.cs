using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoCtrl : MonoBehaviour
{
    public float totalSize;
    public float size;//隕石の大きさ
    public float safeSize = 5;//地球にダメージを与えない最大の大きさ
    public float damage = 1;//基礎ダメージ
    public int point = 100;//基礎加点スコア
    private GameObject target;
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject color;//色を変える部分
    [SerializeField]
    private GameObject effect;//分裂時のエフェクト
    public bool isShot;
    public bool isCaught;

    public bool isHit;
    private float timer;

    // Start is called before the first frame update
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Earth");
        //子オブジェクトがあればサイズを合計
        if (transform.childCount == 0)
            totalSize = size;
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                totalSize += transform.GetChild(i).GetComponent<MeteoCtrl>().size;
                transform.GetChild(i).GetComponent<Rigidbody2D>().isKinematic = true;
            }
        }

        //メテオキャッチ
        isCaught = false;
        isShot = false;
        isHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.childCount ==1)
        {
            var child = transform.GetChild(0).gameObject;
                child.layer = 8;
            child.transform.parent = null;

        }
        //ColorChange();
        Move();
        Death();
        if (isShot)
            timer++;
        if (timer >= 180)
        {
            isShot = false;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        }           
    }

    public void SetSize(float size)
    {
        this.size = size;
        
    }

    void Move()
    {
        if (isCaught || isShot) return;
        if (target.transform.position != transform.position && totalSize >= safeSize) 
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
    }
    
    void Division()
    {
        //隕石を分離させる
        gameObject.layer = 8;
        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<CapsuleCollider2D>().isTrigger = false;
        transform.parent = null;
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

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Meteo" && isShot)
        {
            MeteoCtrl otherMeteo = col.gameObject.GetComponent<MeteoCtrl>();
            otherMeteo.Division();
            isShot = false;
        }

        if (col.gameObject.tag == "stage")
        {
            isHit = true;
            //Destroy(gameObject);
        }

    }
}