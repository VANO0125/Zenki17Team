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
    private int meteoNum;
    private int posNum;
    [SerializeField]
    private EarthCtrl earth;
    MeteoTable meteoTable;
    int meteoCnt = 0;
    int spawnNum;
    private List<float> spawnTime = new List<float>();



    // Start is called before the first frame update
    void Start()
    {
        spawnNum = 0;
        meteoTable = new MeteoTable();
        meteoTable.Load();
        foreach (var enemyMaster in meteoTable.All)
        {
            spawnTime.Add(enemyMaster.TIME);
        }

    }

    // Update is called once per frame
    void Update()
    {      
        if (!earth.isDead)
        {
            SpawnPos();
            Spawn();
        }
    }

    void Spawn()
    {
        timer += Time.deltaTime;
        if (timer >= spawnTime[spawnNum])
        {
            MeteoCtrl newMeteo = Instantiate(meteos[meteoNum], train.position, Quaternion.identity) as MeteoCtrl;
            if (spawnNum < spawnTime.Count)
            {
                spawnNum++;
            }         
            meteoCnt++;
            Debug.Log("メテオ" + meteoCnt);
            newMeteo.earth = earth;
            newMeteo.SetTarget(earth.transform);
        }

    }

    void SpawnPos()
    {
        posNum = Random.Range(0, spawnPos.Count);
        meteoNum = Random.Range(0, meteos.Count);
        train = spawnPos[posNum].transform;
    }
}
