using System.Collections.Generic;
using UnityEngine;

public class BarrierManager : SimpleSingleton<BarrierManager>
{
    [SerializeField]
    List<BarrierNode> barrierData;

    [SerializeField]
    float alpha;


    Dictionary<InvisBarrier.BarrierType, Color> barrierMap;

    public void Start()
    {
        if (alpha == 0f)
        {
            alpha = 0.1f;
        }

        // create a true map from the serializable class 
        barrierMap = new Dictionary<InvisBarrier.BarrierType, Color>();
        foreach (BarrierNode node in barrierData)
        {
            Color c = node.color;
            c.a = alpha;
            barrierMap[node.barrierType] = c;
        }
    }

    public Color GetColor(InvisBarrier.BarrierType type)
    {
        if (barrierMap == null)
            return new Color();
        else
            return barrierMap[type];
    }
}
