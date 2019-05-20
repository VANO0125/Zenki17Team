using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoTable : CSVLoader<EnemyMaster>
{
    private static readonly string FilePath = "EnemyMaster";
    public void Load() { Load(FilePath); }

}

public class EnemyMaster : MasterBase
{
    public int ID { get; private set; }
    public float TIME { get; private set; }
   
}

