using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

static public class ObjectMove
{
    static int _xMax;
    static int _yMax;


    static Grid[,] _grids = GridManager.Instance.Grids;
    static (int, int)[] _hexMoveEven = { (0, 1), (1, 0), (0, -1), (-1, -1), (-1, 0), (-1, 1) };
    static (int, int)[] _hexMoveOdd = { (1, 1), (1, 0), (1, -1), (0, -1), (-1, 0), (0, 1) };

    static public float objMoveSpeed = 0.1f;

    static (int, int)[] HexMove(int y)
    {
        if (y % 2 == 0)
            return _hexMoveEven;
        else
            return _hexMoveOdd;
    }
    static (int, int)[] HexMove(float y)
    {
        if (y % 2 == 0)
            return _hexMoveEven;
        else
            return _hexMoveOdd;
    }
    static public void MoveRange(ObjectSO obj)
    {
        _xMax = GridManager.Instance.Stage.Size.x;
        _yMax = GridManager.Instance.Stage.Size.y;

        Queue<(int, int, int)> queue = new Queue<(int, int, int)>();
        queue.Enqueue(((int)obj.Pos.x, (int)obj.Pos.y, 0));
        while (queue.Count > 0)
        {
            (int, int, int) deque = queue.Dequeue();
            _grids[deque.Item1, deque.Item2].MoveRange = true;
            if (deque.Item3 == obj.Step) continue;
            foreach ((int, int) moveDir in HexMove(deque.Item2))
            {
                if (MoveCheck(deque.Item1 + moveDir.Item1, deque.Item2 + moveDir.Item2))
                    queue.Enqueue((deque.Item1 + moveDir.Item1, deque.Item2 + moveDir.Item2, deque.Item3 + 1));
            }
        }


        bool MoveCheck(int xValue, int yValue)
        {
            if (!CheckX(xValue)) return false;//xがグリッド上にないならfalse
            if (!CheckY(yValue)) return false;//yがグリッド上にないならfalse
            if (_grids[xValue, yValue].MoveRange) return false;//すでに判定しているならfalse
            if ((int)obj.Pos.x == xValue && (int)obj.Pos.y == yValue) return false;//原点ならfalse
            if (obj.Type == ObjectType.Player && _grids[xValue, yValue].State == GridState.Enemy) return false;
            if (obj.Type == ObjectType.Enemy && _grids[xValue, yValue].State == GridState.Player) return false;
            if (_grids[xValue, yValue].State == GridState.Obstacle) return false;
            return true;
        }
    }
    static public void MoveRange(ObjectSO obj, out List<Grid> IsMoves)
    {
        IsMoves = new List<Grid>();
        _xMax = GridManager.Instance.Stage.Size.x;
        _yMax = GridManager.Instance.Stage.Size.y;

        Queue<(int, int, int)> queue = new Queue<(int, int, int)>();
        queue.Enqueue(((int)obj.Pos.x, (int)obj.Pos.y, 0));
        while (queue.Count > 0)
        {
            (int, int, int) deque = queue.Dequeue();
            _grids[deque.Item1, deque.Item2].MoveRange = true;
            IsMoves.Add(_grids[deque.Item1, deque.Item2]);
            if (deque.Item3 == obj.Step) continue;
            foreach ((int, int) moveDir in HexMove(deque.Item2))
            {
                if (MoveCheck(deque.Item1 + moveDir.Item1, deque.Item2 + moveDir.Item2))
                    queue.Enqueue((deque.Item1 + moveDir.Item1, deque.Item2 + moveDir.Item2, deque.Item3 + 1));
            }
        }


        bool MoveCheck(int xValue, int yValue)
        {
            if (!CheckX(xValue)) return false;//xがグリッド上にないならfalse
            if (!CheckY(yValue)) return false;//yがグリッド上にないならfalse
            if (_grids[xValue, yValue].MoveRange) return false;//すでに判定しているならfalse
            if ((int)obj.Pos.x == xValue && (int)obj.Pos.y == yValue) return false;//原点ならfalse
            if (obj.Type == ObjectType.Player && _grids[xValue, yValue].State == GridState.Enemy) return false;
            if (obj.Type == ObjectType.Enemy && _grids[xValue, yValue].State == GridState.Player) return false;
            if (_grids[xValue, yValue].State == GridState.Obstacle) return false;
            return true;
        }
    }

    static bool CheckX(int value)
    {
        if (value >= _xMax) return false;
        if (value < 0) return false;
        return true;
    }
    static bool CheckY(int value)
    {
        if (value >= _yMax) return false;
        if (value < 0) return false;
        return true;
    }

    static public void OnMoveClear()
    {
        foreach (Grid g in _grids)
        {
            g.MoveRange = false;
        }
    }

    static public void OnAttackClear()
    {
        foreach (Grid g in _grids)
        {
            g.AttackRange = false;
        }
    }

    static public void AttackRange(ObjectSO obj)
    {
        foreach ((int, int) moveDir in HexMove(obj.Pos.y))
        {
            _grids[(int)obj.Pos.x + moveDir.Item1, (int)obj.Pos.y + moveDir.Item2].AttackRange = true;
        }
    }
    /// <summary>ゴール座標と動かすObjectSOを渡せばそこまでのルートを返す</summary>
    /// <returns>スタートからゴールまでの座標</returns>

    static public Stack<(int, int)> AStarSearch(int goalX, int goalY, ObjectSO obj)
    {
        int startX = (int)obj.Pos.x;
        int startY = (int)obj.Pos.y;
        AstarGrid[,] astarGrids = new AstarGrid[_xMax, _yMax];
        for (int y = 0; y < _yMax; y++)
            for (int x = 0; x < _xMax; x++)
                astarGrids[x, y] = new AstarGrid(x, y);
        if (obj.Type == ObjectType.Player)
        {
            foreach (ObjectSO blocker in GameManager.Instance.Enemys)
            {
                astarGrids[(int)blocker.Pos.x, (int)blocker.Pos.y].State = AstarGrid.AstarState.Block;
            }
        }
        else
        {
            foreach (ObjectSO blocker in GameManager.Instance.Players)
            {
                astarGrids[(int)blocker.Pos.x, (int)blocker.Pos.y].State = AstarGrid.AstarState.Block;
            }
        }
        foreach (ObjectSO blocker in GameManager.Instance.Obstacles)
        {
            astarGrids[(int)blocker.Pos.x, (int)blocker.Pos.y].State = AstarGrid.AstarState.Block;
        }
        astarGrids[startX, startY].State = AstarGrid.AstarState.Start;
        astarGrids[goalX, goalY].State = AstarGrid.AstarState.Goal;

        Vector2 goalPos = new Vector2(_grids[goalX, goalY].WorldPos.x, _grids[goalX, goalY].WorldPos.z);

        List<AstarGrid> openGrid = new List<AstarGrid> { astarGrids[startX, startY] };

        bool isGoal = false;
        while (!isGoal)
        {
            AstarGrid currentGrid = openGrid[0];
            openGrid.RemoveAt(0);

            foreach ((int, int) moveDir in HexMove(currentGrid.Pos.Item2))
            {
                int nextX = currentGrid.Pos.Item1 + moveDir.Item1;
                int nextY = currentGrid.Pos.Item2 + moveDir.Item2;
                if (CheckX(nextX) && CheckY(nextY))
                {
                    AstarGrid nextGrid = astarGrids[nextX, nextY];
                    if (nextGrid.State == AstarGrid.AstarState.Goal)
                    {
                        nextGrid.BeforePos = currentGrid.Pos;
                        isGoal = true;
                    }
                    //まだGrid上に物があるかの判定をしていない
                    if (nextGrid.State != AstarGrid.AstarState.Open
                        && nextGrid.State != AstarGrid.AstarState.Start
                        && nextGrid.State != AstarGrid.AstarState.Block)
                    {
                        Vector2 nextWorldPos = new Vector2(_grids[nextX, nextY].WorldPos.x, _grids[nextX, nextY].WorldPos.z);
                        nextGrid.State = AstarGrid.AstarState.Open;
                        nextGrid.BeforePos = currentGrid.Pos;
                        nextGrid.PreCost = Vector2.Distance(nextWorldPos, goalPos);
                        nextGrid.RealCost = currentGrid.RealCost + 1;
                        nextGrid.Score = nextGrid.PreCost + nextGrid.RealCost;
                        openGrid.Add(nextGrid);
                    }
                }
            }
            openGrid.Sort((a, b) => (int)(a.Score.CompareTo(b.Score)));
        }
        AstarGrid current = astarGrids[goalX, goalY];
        Stack<(int, int)> route = new Stack<(int, int)>();
        while (current.State != AstarGrid.AstarState.Start)
        {
            route.Push(current.Pos);
            current = astarGrids[current.BeforePos.Item1, current.BeforePos.Item2];
        }
        return route;
    }

    static public void ObjectPosMove(Stack<(int, int)> route, ObjectSO obj)
    {
        if (route.Count > 0)
        {
            (int, int) pos = route.Pop();
            Vector3 nextPos = GridManager.Instance.Grids[pos.Item1, pos.Item2].WorldPos;
            Vector3 Fo = (nextPos - obj.Object.transform.position);
            Fo.y = 0;
            obj.Object.transform.forward = Fo;
            obj.Object.transform.DOMove(nextPos, objMoveSpeed).OnComplete(() => { ObjectPosMove(route, obj); });
        }
        else
        {
            GameManager.Instance.SelectGrid = _grids[(int)obj.Pos.x, (int)obj.Pos.y];
            if (obj.Type == ObjectType.Player)
                GameManager.Instance.GameState = GameState.SelectObject;
        }
    }
}

public class AstarGrid
{
    public AstarState State;
    public (int, int) Pos;
    /// <summary>前のグリッドの座標</summary>
    public (int, int) BeforePos;
    /// <summary>ゴールまでの距離</summary>
    public float PreCost;
    /// <summary>スタートからの歩数</summary>
    public int RealCost = 0;
    /// <summary>上二つの合計</summary>
    public float Score = 0;
    public enum AstarState
    {
        Way,
        Block,
        Open,
        Start,
        Goal
    }
    public AstarGrid(int x, int y)
    {
        Pos = (x, y);
    }
}