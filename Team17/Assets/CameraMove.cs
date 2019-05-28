using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using GamepadInput;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera cam;
    private Transform lookAt;
    private Transform folow;

    // Start is called before the first frame update
    void Start()
    {
        //   cam = GetComponent<>
        lookAt = cam.LookAt;
        folow = cam.Follow;
    }

    // Update is called once per frame
    void Update()
    {
        folow.position += (Vector3)GamePad.GetAxis(GamePad.Axis.RightStick, GamePad.Index.Any);
    }
}
