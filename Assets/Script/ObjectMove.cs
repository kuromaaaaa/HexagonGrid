using System.Collections.Generic;
using UnityEngine;

static public class ObjectMove
{
    static int _xMax;
    static int _yMax;

    static (int, int)[] _hexMoveEven = { (1, 0), (-1, 0), (0, +1), (0, -1), (-1, -1), (-1, +1) };
    static (int, int)[] _hexMoveOdd = { (1, 0), (-1, 0), (0, +1), (0, -1), (+1, +1), (+1, -1) };
    static public void MoveRange(int x, int y, int step)
    {
        Grid[,] grids = GridManager.Instance.Grids;
        _xMax = GridManager.Instance.SizeX;
        _yMax = GridManager.Instance.SizeY;

        Queue<(int, int, int)> queue = new Queue<(int, int, int)>();
        queue.Enqueue((x, y, 0));
        while (queue.Count > 0)
        {
            (int, int, int) deque = queue.Dequeue();
            grids[deque.Item1, deque.Item2].OnMove = true;
            if (deque.Item3 == step) continue;
            foreach ((int, int) moveDir in deque.Item2 % 2 == 0 ? _hexMoveEven : _hexMoveOdd)
            {
                if (MoveCheck(deque.Item1 + moveDir.Item1, deque.Item2 + moveDir.Item2))
                    queue.Enqueue((deque.Item1 + moveDir.Item1, deque.Item2 + moveDir.Item2, deque.Item3 + 1));
            }
        }

        bool MoveCheck(int xValue, int yValue)
        {
            if (!CheckX(xValue)) return false;//xがグリッド上にないならfalse
            if (!CheckY(yValue)) return false;//yがグリッド上にないならfalse
            if (grids[xValue, yValue].OnMove) return false;//すでに判定しているならfalse
            if (x == xValue && y == yValue) return false;//原点ならfalse
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
        Grid[,] grids = GridManager.Instance.Grids;
        foreach (Grid g in grids)
        {
            g.OnMove = false;
        }
    }

    static public void AStarSearch(int startX, int startY, int goalX, int goalY)
    {
        AstarGrid[,] astarGrids = new AstarGrid[_xMax, _yMax];
        for (int y = 0; y < _yMax; y++)
            for (int x = 0; x < _xMax; x++)
                astarGrids[x, y] = new AstarGrid(x, y);
        astarGrids[startX, startY].State = AstarGrid.AstarState.Start;
        astarGrids[goalX, goalY].State = AstarGrid.AstarState.Goal;
        Grid[,] grids = GridManager.Instance.Grids;
        Vector2 goalPos = new Vector2(grids[goalX, goalY].WorldPos.x, grids[goalX, goalY].WorldPos.z);

        List<AstarGrid> openGrid = new List<AstarGrid> { astarGrids[startX, startY] };

        bool isGoal = false;
        while (!isGoal)
        {
            AstarGrid currentGrid = openGrid[0];
            openGrid.RemoveAt(0);

            foreach ((int, int) moveDir in currentGrid.Pos.Item2 % 2 == 0 ? _hexMoveEven : _hexMoveOdd)
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
                    if (nextGrid.State != AstarGrid.AstarState.Open && nextGrid.State != AstarGrid.AstarState.Start)
                    {
                        Vector2 nextWorldPos = new Vector2(grids[nextX, nextY].WorldPos.x, grids[nextX, nextY].WorldPos.z);
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
        Debug.Log(string.Join("⇒", route));
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
        Close,
        Start,
        Goal
    }
    public AstarGrid(int x, int y)
    {
        Pos = (x, y);
    }
}