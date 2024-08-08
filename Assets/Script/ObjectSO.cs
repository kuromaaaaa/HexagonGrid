using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ObjectSO")]
public class ObjectSO : ScriptableObject
{
    public ObjectType Type;
    public AttackType AttackType;
    public Vector2 Pos;
    public string ObjectName;
    public int Hp;
    public int Attack;
    public int Step;
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