using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    GameState _gameState;
    public GameState GameState
    {
        get => _gameState;
        set { _gameState = value; Debug.Log(value); }
    }
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
            if (_gameState != GameState.EnemyTurn)
            {
                SelectGridChange(_selectGrid, value, out Grid newSelect);

                _selectGrid = newSelect;
            }
        }
    }

    Transform _cm;

    [SerializeField] ObjectSO _obstacle;
    public ObjectSO Obstacle { get => _obstacle; }

    [SerializeField] List<ObjectSO> _players = new List<ObjectSO>();
    public List<ObjectSO> Players { get => _players; set { _players = value; } }
    [SerializeField] List<ObjectSO> _enemys = new List<ObjectSO>();
    public List<ObjectSO> Enemys { get => _enemys; set { _enemys = value; } }
    private void Start()
    {
        _cm = Camera.main.transform;
    }

    void SelectGridChange(Grid beforeSelect, Grid selectGrid, out Grid newSelect)
    {
        newSelect = null;
        //カメラ移動
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
                if (selectGrid.MoveRange)
                {
                    int sx = (int)selectGrid.GridPos.x;
                    int sy = (int)selectGrid.GridPos.y;
                    Stack<(int, int)> route = ObjectMove.AStarSearch(sx, sy, beforeSelect.OnObject);
                    Debug.Log($"({beforeSelect.GridPos.x}, {beforeSelect.GridPos.y})⇒" + string.Join("⇒", route));
                    ObjectMove.OnMoveClear();
                    ObjectMove.ObjectPosMove(route, beforeSelect.OnObject);
                    if (selectGrid.State == GridState.Goal)
                    {
                        Debug.Log("ゴール");
                        GameObject fadeOut = UIManager.Instance.FadeOut;
                        fadeOut.SetActive(true);
                        fadeOut.GetComponent<FadeOut>().GameClear += (() => SceneManager.LoadScene("SampleScene"));
                        break;
                    }
                    UIManager.Instance.SelectObject(SelectGrid.OnObject);
                    beforeSelect.OnObject.IsMove = false;
                    selectGrid.OnObject = beforeSelect.OnObject;
                    beforeSelect.OnObject = null;
                    GameState = GameState.Move;
                }
                else
                {
                    GameState = GameState.None;
                }
                break;
            }
            case (GameState.AttackRangeSearch):
            {
                if ((selectGrid.OnObject && selectGrid.OnObject.Type == ObjectType.Enemy) && selectGrid.AttackRange)
                {
                    ObjectSO first = beforeSelect.OnObject;
                    ObjectSO second = selectGrid.OnObject;
                    Debug.Log($"バトル {beforeSelect.OnObject.name} ⇒ {selectGrid.OnObject.name}");
                    second.AddHP(first.Attack * -1);
                    if (second.IsAlive)
                    {
                        first.AddHP(second.Attack * -1);
                    }
                    else
                    {
                        UIManager.Instance.SelectCancel();
                    }
                    first.IsAttack = false;
                    first.IsMove = false;
                    TurnEndCheck();
                }
                GameState = GameState.None;
                ObjectMove.OnMoveClear();
                ObjectMove.OnAttackClear();
                newSelect = beforeSelect;
                UIManager.Instance.SelectObject(beforeSelect.OnObject);
                break;
            }
        }
        if (!selectGrid.OnObject)
        {
            GameState = GameState.None;
            ObjectMove.OnMoveClear();
            ObjectMove.OnAttackClear();
        }
        if (newSelect == null)
        {
            newSelect = selectGrid;
        }
    }

    public void MoveButton()
    {
        if (SelectGrid.OnObject.IsMove)
        {
            GameState = GameState.MoveRangeSearch;
            ObjectMove.MoveRange(SelectGrid.OnObject);
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
    public void MoveSkipButton()
    {
        SelectGrid.OnObject.IsMove = false;
        SelectGrid.OnObject.IsAttack = false;
        UIManager.Instance.SelectObject(SelectGrid.OnObject);
        TurnEndCheck();
    }

    bool IsSelectGridCameraMove(Grid selectGrid)
    {
        if (_gameState == GameState.AttackRangeSearch &&
            (selectGrid.OnObject && selectGrid.OnObject.Type == ObjectType.Enemy)
            && selectGrid.AttackRange)
            return false;
        return true;
    }

    public void PlayerTurnStart()
    {
        foreach (ObjectSO player in _players)
        {
            player.IsMove = true;
            player.IsAttack = true;
        }
    }

    void TurnEndCheck()
    {
        bool isTurnEnd = true;
        foreach (ObjectSO obj in _players)
        {
            if (obj.IsAttack)
            {
                isTurnEnd = false;
            }
        }
        if (isTurnEnd)
            EnemyTurnStart();
    }


    public void EnemyTurnStart()
    {
        if (GameState != GameState.EnemyTurn)
        {
            GameState = GameState.EnemyTurn;
            foreach (ObjectSO enemy in _enemys)
            {
                enemy.IsMove = true;
                enemy.IsAttack = true;
            }
            EnemyTurn();
        }
    }
    public async void EnemyTurn()
    {
        Grid[,] grids = GridManager.Instance.Grids;
        foreach (ObjectSO enemy in _enemys)
        {
            ObjectMove.MoveRange(enemy, out List<Grid> isMoves);
            ObjectSO targetPlayer = _players[0];
            float targetDistance = float.MaxValue;
            //一番近いプレイヤーを探す
            foreach (ObjectSO player in _players)
            {
                Vector3 dir = grids[(int)targetPlayer.Pos.x, (int)targetPlayer.Pos.y].WorldPos
                    - grids[(int)enemy.Pos.x, (int)enemy.Pos.y].WorldPos;
                if (targetDistance > dir.sqrMagnitude)
                {
                    targetPlayer = player;
                    targetDistance = dir.sqrMagnitude;
                }
            }
            Grid moveTargetGrid = grids[(int)enemy.Pos.x, (int)enemy.Pos.y];
            float EtoG = float.MaxValue;
            float GtoP = float.MaxValue;
            //ターゲットのプレイヤーに一番近いグリッドを探す
            foreach (Grid g in isMoves)
            {
                if (g.OnObject && g.OnObject.Type == ObjectType.Enemy)
                    continue;
                float searchEtoG = (g.WorldPos - grids[(int)enemy.Pos.x, (int)enemy.Pos.y].WorldPos).sqrMagnitude;
                float searchGtoP = (grids[(int)targetPlayer.Pos.x, (int)targetPlayer.Pos.y].WorldPos - g.WorldPos).sqrMagnitude;
                if (GtoP > searchGtoP && !g.OnObject)
                {
                    moveTargetGrid = g;
                    EtoG = searchEtoG;
                    GtoP = searchGtoP;
                }
                else if (EtoG == searchEtoG && !g.OnObject)
                {
                    if (GtoP > searchGtoP)
                    {
                        moveTargetGrid = g;
                        EtoG = searchEtoG;
                        GtoP = searchGtoP;
                    }
                }
            }
            //ルートの探索と移動
            Stack<(int, int)> route = ObjectMove.AStarSearch((int)moveTargetGrid.GridPos.x, (int)moveTargetGrid.GridPos.y, enemy);
            ObjectMove.ObjectPosMove(route, enemy);
            int moveStep = route.Count;
            await UniTask.Delay((int)(ObjectMove.objMoveSpeed * (moveStep + 1) * 1000));


            grids[(int)enemy.Pos.x, (int)enemy.Pos.y].OnObject = null;
            moveTargetGrid.OnObject = enemy;

            UIManager.Instance.SelectObject(enemy);



            ObjectMove.OnMoveClear();
        }
        PlayerTurnStart();
        GameState = GameState.None;
    }
}