using UnityEngine;

public class Grid : MonoBehaviour
{
    Vector3 _pos;
    public Vector3 Pos
    {
        get { return _pos; }
    }

    GridState _state;
    public GridState State
    {
        get { return _state; }
        set
        {
            _state = value;
            //グリッドの色変える
            _mr.material.color = _gridColor[(int)value];
        }
    }

    bool onObject;
    public bool OnObject 
    {
        get { return onObject; }
        set { onObject = value; }
    }

    [SerializeField] int num = 0;

    [SerializeField] Color[] _gridColor;

    MeshRenderer _mr;

    bool OnStarted;
    private void Start()
    {
        _mr = GetComponent<MeshRenderer>();
        _pos = transform.position;
        OnStarted = true;
    }

    void OnValidate()
    {
        if(OnStarted)
            State = (GridState)num;
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
    Select
}