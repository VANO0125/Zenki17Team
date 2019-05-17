using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoManger : MonoBehaviour
{
    private Transform train;
    [SerializeField]
    private List<GameObject> spawnPos;
    [SerializeField]
    private List<MeteoCtrl> meteos;
    private float timer;
    public float timermax;
    private int meteoNum;
    private int posNum;
    [SerializeField]
    private EarthCtrl earth;
    // Start is called before the first frame update
    void Start()
    {
        //最初一個スポーン
        //SpawnPos();
        //posNum = Random.Range(0, spawnPos.Count);
        //MeteoCtrl newMeteo = Instantiate(meteos[meteoNum], train.position, Quaternion.identity) as MeteoCtrl;
        //timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!earth.isDead)
        {
            SpawnPos();
            Spawn();
        }
       
    }

    void Spawn()
    {
        //メテオ数
        timer += Time.deltaTime;
        if (timer >= timermax)
        {
            MeteoCtrl newMeteo = Instantiate(meteos[meteoNum], train.position, Quaternion.identity)as MeteoCtrl;
            newMeteo.earth = earth;
            newMeteo.SetTarget(earth.transform);
            timer = 0;
        }
    }

    void SpawnPos()
    {
        posNum = Random.Range(0, spawnPos.Count);
        meteoNum = Random.Range(0, meteos.Count);
        train = spawnPos[posNum].transform;
    }
}
