using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class MeteoLayer :SingletonMonoBehaviour<MeteoLayer>
{
    bool[] flags;
    private int layerNum;
   
    // Start is called before the first frame update
    void Start()
    {
        flags = new bool[20];
        
    }

    public int GetLayer()
    {
        for (int i = 0; i < flags.Length; i++)
        {
            layerNum = 11;
            if (!flags[i])
            {
                flags[i] = true;

                return layerNum + i;
            }

        }
        return 8;
    }
}
