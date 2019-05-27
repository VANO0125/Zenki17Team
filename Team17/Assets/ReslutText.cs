using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReslutText : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private EarthCtrl earth;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (earth.isDead)
        {
            float time = earth.gameTimer;
            int minute = (int)time / 60;//分.timeを60で割った値.
            int second = (int)time % 60;//秒.timeを60で割った余り.
            int mini = (int)((time % 60 - second) * 100);

            text.text = "Time：" + minute + ":" + second + ":" + mini
                        + "\n Score：" + earth.score
                        + "\n" + earth.starCounter + " Shooting Stars";
        }
    }
}
