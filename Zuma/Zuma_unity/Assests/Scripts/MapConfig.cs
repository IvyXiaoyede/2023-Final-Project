﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapConfig : ScriptableObject
{
    public List<Vector3> pathPointList = new List<Vector3>();

    public Vector3 GetPosition(float progress)
    {
        int index = Mathf.FloorToInt(progress);
        return Vector3.Lerp(pathPointList[index], pathPointList[index + 1], progress - index);
    }
    public void InitMapConfig()
    {
        EndPoint = pathPointList.Count - 2;
    }
    public float EndPoint { get; private set; }
}
