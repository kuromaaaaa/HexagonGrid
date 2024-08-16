using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObjects/StageData"),Serializable]
public class StageDataSO : ScriptableObject
{
    public int SizeX;
    public int SizeY;
    public List<Vector2> PlayerStart;
    public List<Vector2> ObstaclePos;
    public Vector2 Goal;
}
