using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GamepadInput;

public class PlayerCtrl : MonoBehaviour
{
    private Rigidbody2D rig;
    public float speed;//移動スピード
    private float plusAnle;
    public int level;  // レベル
    public int exp; // 経験値
    public int nextExpBase; // 次のレベルまでに必要な経験値の基本値
    public int nextExpInterval; // 次のレベルまでに必要な経験値の増加値
    public int prevNeedExp; // 前のレベルに必要だった経験値
    public int needExp; // 次のレベルに必要な経験値
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
    private int layerMask = 1 << 8; //Meteoレイヤーにだけ反応するようにする
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
        //rig.velocity = vec * speed;//スティックでプレイヤー移動
        if (rig.velocity.magnitude >= speed && vec != Vector2.zero)
            rig.velocity = rig.velocity.normalized * speed;

        rig.AddForce(vec * speed);
        
        if (axis != 0)
        {
            transform.Rotate(0, 0, -axis * 1.5f);
        }

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
        Vector2 vec = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any);
        //移動方向を向く処理
        if (vec != Vector2.zero)
        {
            float angle = Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg;
            Quaternion rotation = new Quaternion();
            rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, -angle), 10f);
            //rotation.eulerAngles = new Vector3(0, 0, angle - 90);
            transform.rotation = rotation;
        }
    }

    public void AddExp(int exp)
    {
        this.exp += exp;

    }

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
            if (meteo.GetParent() == null)
            {
                Debug.Log("true");
                catchMeteo = meteo;
                meteo.transform.parent = transform;
                catchMeteo.GetComponent<Rigidbody2D>().simulated = false;
            }
            else if (meteo.GetTotalSize() >= 1)
            {
                meteo.Damage(power * 0.1f);
                meteo.DamageEffect(meteoHit.point);
                audioSource.PlayOneShot(punchSE);
            }
        }
    }

    void MeteoThrow()
    {
        //Bボタンを離すと前方に隕石を投げる
        //if (meteoCounter > 0)
        //{
        //    if (GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.Any))
        //    {
        //        meteoCounter--;
        //        MeteoCtrl shotMeteo = Instantiate(meteo, transform.position + transform.up * 3, Quaternion.identity);
        //        shotMeteo.ShotMeteo(transform.up, shotPower, power, transform);
        //        audioSource.PlayOneShot(throwSE);
        //    }
        //else if (GamePad.GetButton(GamePad.Button.RightShoulder, GamePad.Index.Any))
        //{
        //    rushTimer++;
        //    if (rushTimer > rushInterval)
        //    {
        //        meteoCounter--;
        //        MeteoCtrl shotMeteo = Instantiate(meteo, transform.position + transform.up * 2, Quaternion.identity);
        //        shotMeteo.ShotMeteo(transform.up, shotPower, power, transform);
        //        audioSource.PlayOneShot(throwSE);
        //        rushTimer = 0;
        //        rushInterval--;
        //        if (rushInterval <= 0)
        //            rushInterval = 0;
        //    }
        //}
        //else rushInterval = 10;
        //}
        if (catchMeteo != null && !GamePad.GetButton(GamePad.Button.B, GamePad.Index.Any))
        {
            catchMeteo.ShotMeteo(transform.up, shotPower, power, transform);
            catchMeteo = null;
        }
    }
}

