using UnityEngine;

public class Grid : MonoBehaviour
{
    Vector3 _worldPos;
    /// <summary>ゲームオブジェクトがゲーム画面上に配置された場所</summary>
    public Vector3 WorldPos
    {
        get { return _worldPos; }
        set { _worldPos = value; }
    }

    Vector2 _gridPos;
    public Vector2 GridPos
    {
        get { return _gridPos; }
        set { _gridPos = value; }
    }

    GridState _state;
    public GridState State
    {
        get { return _state; }
        set
        {
            _state = value;
            //グリッドの色変える
            GetComponent<MeshRenderer>().material.color = GridManager.Instance.GridColor[(int)value];
        }
    }
    ObjectSO _onObject;
    public ObjectSO OnObject 
    {
        get { return _onObject; }
        set 
        { 
            _onObject = value;
            if (value == null)
                State = GridState.None;
            else
            {
            value.Pos = _gridPos;
                switch (value.Type)
                {
                    case (ObjectType.Player):
                    {
                        State = GridState.Player;break;
                    }
                    case (ObjectType.Enemy):
                    {
                        State = GridState.Enemy; break;
                    }
                    case (ObjectType.Obstacle):
                    {
                        State = GridState.Obstacle; break;
                    }
                }
            }
        }
    }

    bool _moveRange = false;
    public bool MoveRange
    {
        get { return _moveRange; }
        set 
        {
            _moveRange = value;
            if (value == true)
            {
                if (State == GridState.None)
                    State = GridState.OnMove;
            }
            else
            {
                if (State == GridState.OnMove)
                    State = GridState.None;
            }
        }
    }
    bool _attackRange = false;
    public bool AttackRange
    {
        get { return _attackRange; }
        set
        {
            _attackRange = value;
            if (value == true)
            {
                if (State == GridState.None)
                    State = GridState.OnAttack;
            }
            else
            {
                if (State == GridState.OnAttack)
                    State = GridState.None;
            }
        }
    }
}

public enum GridState
{
    None,
    Player,
    OnMove,
    OnAttack,
    Enemy,
    Obstacle,
    Goal,
}