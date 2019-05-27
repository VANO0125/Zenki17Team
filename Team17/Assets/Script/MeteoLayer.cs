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
        layerNum = 11;
        for (int i = 0; i < flags.Length; i++)
        {

            if (!flags[i])
            {
                flags[i] = true;

                return layerNum + i;
            }


        }
        return 8;
    }

    public void ChangeBool(int Num)
    {
        flags[Num - 11] = false;
    }
}
