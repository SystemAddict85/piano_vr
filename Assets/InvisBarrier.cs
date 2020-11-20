using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisBarrier : MonoBehaviour
{
    [SerializeField]
    BarrierType barrierType;
 
    public enum BarrierType
    {
        DESTROY, SPAWN, TRACK, DEFAULT, BARRIER_COUNT
    }

    public Color destroyColor;

    // Start is called before the first frame update
    void Start()
    {
        if (barrierType == null)
        {
            barrierType = BarrierType.DEFAULT;
        }

        MakeInvisible();

    }

    public void MakeInvisible()
    {
        Color c = GetComponent<MeshRenderer>().material.color;
        c.a = 0.0f;
        GetComponent<MeshRenderer>().material.color = c;
    }

    private void OnDrawGizmos()
    {
        if (BarrierManager.Instance == null)
            return;
        else
        {
            Color c = BarrierManager.Instance.GetColor(barrierType);
            Gizmos.color = c;
            //Debug.Log(name + " : " + c);
            Gizmos.DrawCube(transform.position, transform.lossyScale);
        }        
    }
}
