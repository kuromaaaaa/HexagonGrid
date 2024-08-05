using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

static public class ObjectMove
{
    static public void MoveRange(int x, int y, int step)
    {
        Grid[,] grids = GridManager.Instance.Grids;
        int xMax = GridManager.Instance.SizeX;
        int yMax = GridManager.Instance.SizeY;

        Queue<(int, int, int)> queue = new Queue<(int, int, int)>();
        queue.Enqueue((x, y, 0));
        while (queue.Count > 0)
        {
            (int, int, int) deque = queue.Dequeue();
            grids[deque.Item1, deque.Item2].OnMove = true;
            Debug.Log($"{deque.Item1}, {deque.Item2}");
            if (deque.Item3 == step) continue;
            if (MoveCheck(deque.Item1 + 1, deque.Item2)) queue.Enqueue((deque.Item1 + 1, deque.Item2, deque.Item3 + 1));
            if (MoveCheck(deque.Item1 - 1, deque.Item2)) queue.Enqueue((deque.Item1 - 1, deque.Item2, deque.Item3 + 1));
            if (MoveCheck(deque.Item1, deque.Item2 + 1)) queue.Enqueue((deque.Item1, deque.Item2 + 1, deque.Item3 + 1));
            if (MoveCheck(deque.Item1, deque.Item2 - 1)) queue.Enqueue((deque.Item1, deque.Item2 - 1, deque.Item3 + 1));
            if (deque.Item2 % 2 == 0)
            {
                if (MoveCheck(deque.Item1 - 1, deque.Item2 - 1)) queue.Enqueue((deque.Item1 - 1, deque.Item2 - 1, deque.Item3 + 1));
                if (MoveCheck(deque.Item1 - 1, deque.Item2 + 1)) queue.Enqueue((deque.Item1 - 1, deque.Item2 + 1, deque.Item3 + 1));
            }
            else
            {
                if (MoveCheck(deque.Item1 + 1, deque.Item2 + 1)) queue.Enqueue((deque.Item1 + 1, deque.Item2 + 1, deque.Item3 + 1));
                if (MoveCheck(deque.Item1 + 1, deque.Item2 - 1)) queue.Enqueue((deque.Item1 + 1, deque.Item2 - 1, deque.Item3 + 1));
            }
        }

        bool MoveCheck(int xValue, int yValue)
        {
            if (!CheckX(xValue)) return false;
            if (!CheckY(yValue)) return false;
            if (grids[xValue, yValue].OnMove) return false;
            if (x == xValue && y == yValue) return false;
            return true;
        }

        bool CheckX(int value)
        {
            if (value >= xMax) return false;
            if (value < 0) return false;
            return true;
        }
        bool CheckY(int value)
        {
            if (value >= yMax) return false;
            if (value < 0) return false;
            return true;
        }
    }

    static public void OnMoveClear()
    {
        Grid[,] grids = GridManager.Instance.Grids;
        foreach (Grid g in grids)
        {
            g.OnMove = false;
        }
    }

    static public void AStarSearch(int startX,int startY ,int goalX,int GoalY)
    {

    }
}

public class AstarGrid
{
    public AstarState State;
    public (int, int) Pos;
    public (int, int) BeforePos;
    public int PreCost;
    public float RealCost = 0;
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
}