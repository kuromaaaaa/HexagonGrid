using UnityEngine;

public class Grid : MonoBehaviour
{
    Vector3 _pos;
    public Vector3 Pos
    {
        get { return _pos; }
        set { _pos = value; }
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
            OnObject = (value == GridState.Player || value == GridState.Enemy || value == GridState.Obstacle)
                ? true : false;
        }
    }

    bool onObject;
    public bool OnObject
    {
        get { return onObject; }
        set { onObject = value; }
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