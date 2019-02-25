using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoManger : MonoBehaviour
{
    public GameObject Meteo;
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

        timer += Time.deltaTime;
        if (timer >= timermax)
        {
            Instantiate(Meteo, train.position, train.rotation);
          
            timer = 0;
        }
    }

    void SpawnPos()
    {
        int posNum = Random.Range(0, spawnPos.Count);
        train = spawnPos[posNum].transform;
    }
}
