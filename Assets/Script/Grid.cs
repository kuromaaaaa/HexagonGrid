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
            GetComponent<MeshRenderer>().material.color = _gridColor[(int)value];
        }
    }
    ObjectSO onObject;
    public ObjectSO OnObject 
    {
        get { return onObject; }
        set 
        { 
            onObject = value;
            if (value == null)
                State = GridState.None;
            else
            {
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

    [SerializeField] Color[] _gridColor;

    bool _onMove = false;
    public bool OnMove
    {
        get { return _onMove; }
        set 
        { 
            _onMove = value;
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
}

public enum GridState
{
    None,
    Player,
    OnMove,
    Enemy,
    Obstacle,
    Goal,
}