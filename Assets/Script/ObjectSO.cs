using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ObjectSO")]
public class ObjectSO : ScriptableObject
{
    public ObjectType Type;
    public AttackType AttackType;
    public string ObjectName;
    public Vector2 Pos;
    public int MaxHp;
    public int Hp;
    public int Attack;
    public int Step;
    public bool IsMove = true;
    public bool IsAttack = true;
    bool _isAlive = true;
    public bool IsAlive 
    {
        get => _isAlive;
        set { Debug.Log("€‚ñ‚¾‚Æ‚«‚Ìˆ—‘‚­");
            _isAlive = value;
        } 
    }

    public GameObject StartObject;
    public GameObject Object;

    public void AddHP(int n )
    {
        Hp += n;
        if(Hp <= 0)
        {
            IsAlive = false;
        }
    }
}

public enum ObjectType
{
    Player,
    Enemy,
    Obstacle
}

public enum AttackType
{
    Sword,
    Axe,
    Spear,
    Bow
}