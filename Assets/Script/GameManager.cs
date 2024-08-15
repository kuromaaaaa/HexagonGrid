using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    GameState _gameState;
    public GameState GameState { set { _gameState = value; Debug.Log(value); } }
    Grid _selectGrid;
    public Grid SelectGrid
    {
        get { return _selectGrid; }
        set
        {
            if (value.OnObject)
            {
                UIManager.Instance.SelectObject(value.OnObject);
            }
            else
            {
                UIManager.Instance.SelectCancel();
            }
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
        //ÉJÉÅÉâà⁄ìÆ
        if (IsSelectGridCameraMove(selectGrid))
        {
            Camera.main.gameObject.transform.
                DOMove(new Vector3(selectGrid.WorldPos.x, _cm.position.y, selectGrid.WorldPos.z), 0.5f)
                .SetEase(Ease.Linear);
        }
        switch (_gameState)
        {
            case (GameState.None):
            {
                if (selectGrid.OnObject)
                {
                    GameState = GameState.SelectObject;
                }
                break;
            }
            case (GameState.SelectObject):
            {
                if (selectGrid.State == GridState.None || (selectGrid.OnObject && selectGrid.OnObject.Type == ObjectType.Enemy))
                {
                    GameState = GameState.None;
                    ObjectMove.OnMoveClear();
                }
                break;
            }
            case (GameState.Move):
            {
                break;
            }
            case (GameState.MoveRangeSearch):
            {
                int bx = (int)beforeSelect.GridPos.x;
                int by = (int)beforeSelect.GridPos.y;
                int sx = (int)selectGrid.GridPos.x;
                int sy = (int)selectGrid.GridPos.y;
                Stack<(int, int)> route = ObjectMove.AStarSearch(bx, by, sx, sy, beforeSelect.OnObject);
                Debug.Log($"({beforeSelect.GridPos.x}, {beforeSelect.GridPos.y})ÅÀ" + string.Join("ÅÀ", route));
                ObjectMove.OnMoveClear();
                ObjectMove.ObjectPosMove(route, beforeSelect.OnObject);
                beforeSelect.OnObject.IsMove = false;
                selectGrid.OnObject = beforeSelect.OnObject;
                beforeSelect.OnObject = null;
                GameState = GameState.Move;
                break;
            }
            case (GameState.AttackRangeSearch):
            {
                if((selectGrid.OnObject && selectGrid.OnObject.Type == ObjectType.Enemy) && selectGrid.AttackRange)
                {
                    ObjectSO first = beforeSelect.OnObject;
                    ObjectSO second = selectGrid.OnObject;
                    Debug.Log($"ÉoÉgÉã {beforeSelect.OnObject.name} ÅÀ {selectGrid.OnObject.name}");
                    second.AddHP(first.Attack * -1);
                    if(second.IsAlive)
                    {
                        first.AddHP(second.Attack * -1);
                    }
                    first.IsAttack = false;
                }
                break;
            }
        }
        if (!selectGrid.OnObject)
        {
            GameState = GameState.None;
            ObjectMove.OnMoveClear();
            ObjectMove.OnAttackClear();
        }
    }

    public void MoveButton()
    {
        if (SelectGrid.OnObject.IsMove)
        {
            GameState = GameState.MoveRangeSearch;
            ObjectMove.MoveRange((int)SelectGrid.GridPos.x, (int)SelectGrid.GridPos.y, SelectGrid.OnObject.Step);
        }
    }
    public void AttackButton()
    {
        if (SelectGrid.OnObject.IsAttack)
        {
            GameState = GameState.AttackRangeSearch;
            ObjectMove.AttackRange(SelectGrid.OnObject);
        }
    }

    bool IsSelectGridCameraMove(Grid selectGrid)
    {
        if (_gameState == GameState.AttackRangeSearch &&
            (selectGrid.OnObject && selectGrid.OnObject.Type == ObjectType.Enemy)
            && selectGrid.AttackRange)
            return false;
        return true;
    }
}
[Serializable]
public class StartObject
{
    public Vector2 StartPosition;
    public ObjectSO ObjectData;
}