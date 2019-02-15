using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class PlayerCtrl : MonoBehaviour
{
    private Rigidbody2D rig;

    public float speed;//移動スピード

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rig.velocity = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any)*speed;//スティックでプレイヤー移動
    }
}
