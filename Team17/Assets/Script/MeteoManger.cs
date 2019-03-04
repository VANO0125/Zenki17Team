using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoManger : MonoBehaviour
{
    public MeteoCtrl Meteo;
    private Transform train;
    [SerializeField]
    private List<GameObject> spawnPos;
    private float timer;
    public float timermax;
    public float meteosize;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;

    }

    // Update is called once per frame
    void Update()
    {
        SpawnPos();
        Spawn();

    }

    void Spawn()
    {
        //メテオ数
        timer += Time.deltaTime;
        if (timer >= timermax)
        {
            MeteoCtrl newMeteo = Instantiate(Meteo, train.position, Quaternion.identity)as MeteoCtrl;
            newMeteo.SetSize(Random.Range(4, 8));
            timer = 0;
        }
    }

    void SpawnPos()
    {
        int posNum = Random.Range(0, spawnPos.Count);
        train = spawnPos[posNum].transform;
    }
}
