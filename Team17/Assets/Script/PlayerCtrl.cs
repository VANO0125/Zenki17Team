using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class PlayerCtrl : MonoBehaviour
{
    private Rigidbody2D rig;
    private bool isCatch;//隕石を掴んでいるか
    private GameObject catchMeteo;//掴んでいる隕石
    public float speed;//移動スピード
   public bool isShot;

    // Start is called before the first frame update
    void Start()
    {
        isShot = false;
        isCatch = false;
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vec = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any);
        rig.velocity = vec * speed;//スティックでプレイヤー移動
        //移動方向を向く処理
        if (vec != Vector3.zero)
        {
            Vector3 pos = rig.velocity;
            float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
            Quaternion rotation = new Quaternion();
            rotation.eulerAngles = new Vector3(0, 0, angle - 90);
            transform.rotation = rotation;
        }
        MeteoCatch();
        MeteoThrow();
    }

    void MeteoCatch()
    {
        //Bボタンで前方の隕石を掴む
        Ray2D catchRay = new Ray2D(transform.position, transform.up); //前方にRayを投射
        int layerMask = 1 << 8; //Meteoレイヤーにだけ反応するようにする
        RaycastHit2D meteoHit = Physics2D.Raycast(catchRay.origin, catchRay.direction, 2, layerMask);
        if (GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any) && catchMeteo == null && meteoHit)
        {
            isCatch = true;      
            catchMeteo = meteoHit.transform.gameObject;
            catchMeteo.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            catchMeteo.GetComponent<Rigidbody2D>().simulated = false;
        }

        //掴んだ隕石を止める
        if (catchMeteo != null)
        { 
            catchMeteo.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            rig.constraints = RigidbodyConstraints2D.FreezePosition;
            catchMeteo.transform.parent = transform;
        }
        else
        {
            isCatch = false;
            rig.constraints = RigidbodyConstraints2D.None;
            rig.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void MeteoThrow()
    {
        //Bボタンを離すと前方に隕石を投げる
        if (isCatch && !GamePad.GetButton(GamePad.Button.B, GamePad.Index.Any))
        {
            catchMeteo.GetComponent<MeteoCtrl>().isShot = true;
            catchMeteo.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            catchMeteo.GetComponent<Rigidbody2D>().simulated = true;
            catchMeteo.transform.parent = null;
            catchMeteo.GetComponent<Rigidbody2D>().AddForce(transform.up * 50, ForceMode2D.Impulse);
            catchMeteo = null;
        }
       
    }


    public bool GetCatch()
    {
        return isCatch;
    }

}
