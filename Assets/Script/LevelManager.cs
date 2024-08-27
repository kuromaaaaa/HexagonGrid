using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    int _level = 0;
    public int Level { get => _level; set { _level = value; } }
    private void Awake()
    {
        DDOL = true;
        base.Awake();
    }
}
