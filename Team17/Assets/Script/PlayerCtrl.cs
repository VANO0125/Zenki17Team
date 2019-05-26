using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GamepadInput;

public class PlayerCtrl : MonoBehaviour
{
    private Rigidbody2D rig;
    public float speed;//移動スピード
    private Vector2 moveAngle;//回転ベクトル
    private float rateTimer;//振り返りのラグ
    public int level;  // レベル
    public int exp; // 経験値
    public int expTable = 10;
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Slider expSlider;


    [SerializeField]
    private float shotPower;
    [SerializeField]
    private float power;//投げた隕石の威力
    private MeteoCtrl catchMeteo;
    private float rushTimer;
    public float rushInterval = 10;
    private int layerMask = -1 - (1 << 0 | 1 << 1 | 1 << 2 | 1 << 4 | 1 << 5 | 1 << 9 | 1 << 10);//Meteoレイヤーにだけ反応するようにする
    public Number scoreNumber;//スコア描写
    public int score;

    public AudioClip throwSE;
    public AudioClip catchSE;
    public AudioClip punchSE;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        expSlider.maxValue = expTable;
        expTable = GetComponent<ExpList>().expList[0];
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vec = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any);
        float axis = Input.GetAxis("R_XAxis_0");
        //スティックでプレイヤー移動
        float size = catchMeteo != null ? catchMeteo.size : 1;
        float rate = speed / size;
        if (rig.velocity.magnitude >= rate && vec != Vector2.zero)        
            rig.velocity = rig.velocity.normalized * rate;
        

        rig.AddForce(vec * speed);


        //レベルアップ処理
        if (exp >= expTable)
        {
            int remainder = exp - expTable;
            exp += remainder;
            power += 5;
            expTable *= 2;
            expSlider.maxValue = expTable;
            exp = 0;
            exp += remainder;
            expTable = GetComponent<ExpList>().expList[level];
            expSlider.maxValue = expTable;
            level++;
        }
        Rotate();
        MeteoCatch();
        MeteoThrow();
        SetUI();
    }

    void Rotate()
    {
        //移動方向を向く処理
        Vector2 vec = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any);
        if (vec != Vector2.zero)
        {
            moveAngle = vec;
            if (Vector2.Angle(transform.up, vec) <= 170)
                rateTimer = 100000;
        }

        float angle = Mathf.Atan2(moveAngle.x, moveAngle.y) * Mathf.Rad2Deg;
        Quaternion rad = Quaternion.Euler(0, 0, -angle);

      //  int rate = catchMeteo != null ? (catchMeteo.size) / 2 : 0;//隕石が大きいほど振り返る速度を遅く
       // rateTimer++;

        //if (rateTimer >= rate * 10)
        //{ 
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, rad, 10f);
            transform.rotation = rotation;
        //}

        //if (transform.rotation == Quaternion.Euler(0, 0, -angle))
        //    rateTimer = 0;
    }

    public void AddExp(int exp)
    { this.exp += exp; }

    void SetUI()
    {
        levelText.text = "Lv." + level;
        expSlider.value = exp;
    }

    void MeteoCatch()
    {
        //Bボタンで前方の隕石を掴む
        Ray2D catchRay = new Ray2D(transform.position, transform.up); //前方にRayを投射
        RaycastHit2D meteoHit = Physics2D.Raycast(catchRay.origin, catchRay.direction, 4, layerMask);
        if (GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any) && meteoHit)
        {
            var meteo = meteoHit.transform.gameObject.GetComponent<MeteoCtrl>();
            //if (meteo.transform.parent==null)
            {
                catchMeteo = meteo.GetHighest();
                meteo.GetHighest().Caught(transform,rig);
                rig.velocity = Vector2.zero;
            }
            //else if (meteo.GetTotalSize() >= 1)
            //{
            //    meteo.Damage(power * 0.1f);
            //    meteo.DamageEffect(meteoHit.point);
            //    audioSource.PlayOneShot(punchSE);
           // }
        }
    }

    void MeteoThrow()
    {
        //Bボタンを離すと前方に隕石を投げる     
        if (catchMeteo != null && !GamePad.GetButton(GamePad.Button.B, GamePad.Index.Any))
        {
            catchMeteo.ShotMeteo(transform.up, shotPower, power, transform);
            catchMeteo = null;
        }
    }
}

