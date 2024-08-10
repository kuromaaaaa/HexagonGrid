using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    GameState gameState;
    public GameState GameState { set { gameState = value; } }
    Grid _selectGrid;
    public Grid SelectGrid
    {
        get { return _selectGrid; }
        set
        {
            SelectGridChange(_selectGrid, value);
            _selectGrid = value;
        }
    }

    Transform _cm;

    [SerializeField] List<StartObject> objects;
    public List<StartObject> Objects { get { return objects; } }

    List<ObjectSO> _players = new List<ObjectSO>();
    public List<ObjectSO> Players { get => _players; set { _players = value; } }
    List<ObjectSO> _enemys = new List<ObjectSO>();
    public List<ObjectSO> Enemys { get => _enemys; set { _enemys = value; } }
    List<ObjectSO> _obstacles = new List<ObjectSO>();
    public List<ObjectSO> Obstacles { get => _obstacles; set { _obstacles = value; } }
    private void Start()
    {
        _cm = Camera.main.transform;
    }

    void SelectGridChange(Grid beforeSelect, Grid selectGrid)
    {
        Camera.main.gameObject.transform.
            DOMove(new Vector3(selectGrid.WorldPos.x, _cm.position.y, selectGrid.WorldPos.z), 0.5f)
            .SetEase(Ease.Linear);

        switch (gameState)
        {
            case (GameState.None):
            {
                if (selectGrid.OnObject)
                {
                    gameState = GameState.SelectObject;
                    if (selectGrid.State == GridState.Player)
                        ObjectMove.MoveRange((int)selectGrid.GridPos.x, (int)selectGrid.GridPos.y, selectGrid.OnObject.Step);
                }
                break;
            }
            case (GameState.SelectObject):
            {
                if (selectGrid.State == GridState.None)
                {
                    gameState = GameState.None;
                    ObjectMove.OnMoveClear();
                }
                if (selectGrid.State == GridState.OnMove)
                {
                    int bx = (int)beforeSelect.GridPos.x;
                    int by = (int)beforeSelect.GridPos.y;
                    int sx = (int)selectGrid.GridPos.x;
                    int sy = (int)selectGrid.GridPos.y;
                    Stack<(int, int)> route = ObjectMove.AStarSearch(bx, by, sx, sy,beforeSelect.OnObject);
                    Debug.Log($"({beforeSelect.GridPos.x}, {beforeSelect.GridPos.y})ÅÀ" + string.Join("ÅÀ", route));
                    ObjectMove.OnMoveClear();
                    ObjectMove.ObjectPosMove(route, beforeSelect.OnObject.Object);
                    selectGrid.OnObject = beforeSelect.OnObject;
                    beforeSelect.OnObject = null;
                    gameState = GameState.Move;
                }
                break;
            }
            case (GameState.Move):
            {
                break;
            }
        }
    }
}
[Serializable]
public class StartObject
{
    public Vector2 StartPosition;
    public ObjectSO ObjectData;
}