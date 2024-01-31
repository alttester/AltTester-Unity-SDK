using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltGameManager : MonoBehaviour
{
    public static AltGameManager Ins;
    public List<Transform> holders;
    public List<AltDragController> dargsObjects;
    private void Awake()
    {
        Ins = this;
    }
    public Transform GetNearestBlock(Vector3 pos)
    {
        float distance = float.MaxValue;
        int index = 0;
        for (int i = 0; i < holders.Count; i++)
        {
            if (!holders[i].GetComponent<AltHolder>().isFull)
            {
                pos.z = holders[i].position.z;
                float dist = Vector3.Distance(pos, holders[i].position);
                if (dist < distance)
                {
                    distance = dist;
                    index = i;
                }
            }
        }
        return holders[index];
    }

    public void ResetGame()
    {
        for (int i = 0; i < dargsObjects.Count; i++)
        {
            dargsObjects[i].ResetObject();
        }
        for (int i = 0; i < holders.Count; i++)
        {
            holders[i].GetComponent<AltHolder>().isFull = false;
        }
    }
}
