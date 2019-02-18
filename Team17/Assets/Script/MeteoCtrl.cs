using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoCtrl : MonoBehaviour
{
    public float size;//隕石の大きさ
    public float safeSize = 3;//地球にダメージを与えない最大の大きさ
    public float damage = 1;//基礎ダメージ

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Earth")
        {
            EarthCtrl earth = col.gameObject.GetComponent<EarthCtrl>();
            //サイズが一定以下なら加点
            if (size <= safeSize)
             earth.AddScore(size); 
            //一定以上ならダメージ
            else
                earth.Damage(size * damage);
            Destroy(gameObject);
        }
    }
}
