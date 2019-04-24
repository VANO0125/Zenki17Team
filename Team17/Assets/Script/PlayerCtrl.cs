using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GamepadInput;

public class PlayerCtrl : MonoBehaviour
{
    private Rigidbody2D rig;
    private bool isCatch;//隕石を掴んでいるか
    private MeteoCtrl catchMeteo;//掴んでいる隕石
    private int layerMask = 1 << 8; //Meteoレイヤーにだけ反応するようにする
    public float speed;//移動スピード
    [SerializeField]
    private float shotPower;
    [SerializeField]
    private Transform catchPos;
    [SerializeField]
    private GameObject trail;

    [SerializeField]
    private MeteoCtrl meteo;
    [SerializeField]
    private Text meteoText;
    private int meteoCounter;
    private float rushTimer;
    public float rushInterval = 10;

    // Start is called before the first frame update
    void Start()
    {
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

        meteoText.text = "×" + meteoCounter;
        MeteoCatch();
        MeteoThrow();

    }

    void MeteoCatch()
    {
        //Bボタンで前方の隕石を掴む
        Ray2D catchRay = new Ray2D(transform.position, transform.up); //前方にRayを投射
        RaycastHit2D meteoHit = Physics2D.Raycast(catchRay.origin, catchRay.direction, 4, layerMask);
        if (GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any) && catchMeteo == null && meteoHit)
        {
            var meteo = meteoHit.transform.gameObject.GetComponent<MeteoCtrl>();
            if (meteo.GetParent() == null)
            {
                //  catchMeteo = meteo;
                //  catchMeteo.Caught(transform);
            }
            else if (meteo.GetTotalSize() >= 1)
                meteo.Damage(1);
        }

        //掴んだ隕石を止める
        if (catchMeteo != null)
            isCatch = true;
        else
            isCatch = false;
    }

    void MeteoThrow()
    {
        //Bボタンを離すと前方に隕石を投げる
        if (meteoCounter > 0)
        {
            if (GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any))
            {
                meteoCounter--;
                MeteoCtrl shotMeteo = Instantiate(meteo, transform.position + transform.up * 3, Quaternion.identity);
                shotMeteo.ShotMeteo(transform.up, shotPower, transform);
            }
            else if (GamePad.GetButton(GamePad.Button.RightShoulder, GamePad.Index.Any))
            {
                rushTimer++;
                if (rushTimer > rushInterval)
                {
                    meteoCounter--;
                    MeteoCtrl shotMeteo = Instantiate(meteo, transform.position + transform.up * 2, Quaternion.identity);
                    shotMeteo.ShotMeteo(transform.up, shotPower, transform);
                    rushTimer = 0;
                    rushInterval--;
                    if (rushInterval >= 5)
                        rushInterval = 5;
                }
                else rushInterval = 10;
            }
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Meteo")
        {
            MeteoCtrl meteo = col.gameObject.GetComponent<MeteoCtrl>();
            if (!meteo.isShot && meteo.GetTotalSize() <= 1)
            {
                Destroy(col.gameObject);
                meteoCounter++;
            }
        }
    }
}
