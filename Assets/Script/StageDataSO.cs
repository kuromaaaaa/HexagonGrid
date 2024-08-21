using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObjects/StageData"),Serializable]
public class StageDataSO : ScriptableObject
{
    public Pos Size;
    public List<Pos> PlayerStart;
    public List<Pos> EnemyStart;
    public List<Pos> ObstaclePos;
    public Pos Goal;

    [Serializable]
    public class Pos
    {
        public int x;
        public int y;

        public Pos(int x, int y) 
        { 
            this.x = x;
            this.y = y;
        }
    }
}
