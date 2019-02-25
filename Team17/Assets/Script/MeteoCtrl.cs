using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoCtrl : MonoBehaviour
{
    public float size;//隕石の大きさ
    public float safeSize = 3;//地球にダメージを与えない最大の大きさ
    public float divisionNum = 2;//分裂数
    public float damage = 1;//基礎ダメージ
    public bool isCaught;
    public int point=100;//基礎加点スコア
    [SerializeField]
    private PlayerCtrl player;
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private float speed;
    Rigidbody2D rb;
    private int hitNum;

    // Start is called before the first frame update
    void Start()
    {
        size=Random.Range(1,5);
        transform.localScale *= size;
        isCaught = false;
        hitNum = 0;
        player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        
    }

    void Move()
    {
        if (isCaught) return;
        rb.AddForceAtPosition((target.transform.position - transform.position).normalized * speed * 0.01f, target.transform.position, ForceMode2D.Impulse);
       
    }

    //スケールとサイズを分裂数分割る
    void Division()
    {
       transform.localScale *= 1/divisionNum;
        size = size / divisionNum;
    }

    private Vector2 CircleHorizon(float radius,Vector2 colPos)
    {
        var angle = Random.Range(0, 360);
        var rad = angle * Mathf.Deg2Rad;
        var px = Mathf.Cos(rad) * radius + colPos.x;
        var py = Mathf.Sin(rad) * radius + colPos.y;
        return new Vector2(px, py);
    }
  

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Earth")
        {
            EarthCtrl earth = col.gameObject.GetComponent<EarthCtrl>();
            //サイズが一定以下なら加点
            if (size <= safeSize)
             earth.AddScore((int)size*point); 
            //一定以上ならダメージ
            else
                earth.Damage(size * damage);
            Destroy(gameObject);
        }

        if(col.gameObject.tag == "Meteo" && player.GetShot())
        {
            Vector2 hitPos;

            Transform spawnpos = transform;
            if(hitNum <=0)
            {
                Division();
            }
           
            foreach (ContactPoint2D point in col.contacts)
            {
                hitPos = point.point;               
            }
            hitNum++;
        }
    }
}
