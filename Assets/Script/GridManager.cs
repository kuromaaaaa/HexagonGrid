using System.Collections.Generic;
using UnityEngine;

public class GridManager : SingletonMonoBehaviour<GridManager>
{
    [SerializeField] List<StageDataSO> stageDatas;
    StageDataSO _currentStage;
    public StageDataSO Stage => _currentStage;
    [SerializeField] GameObject _gridGameObject;
    [SerializeField] GameObject _gridText;
    Grid[,] _grids;
    public Grid[,] Grids => _grids;

    [SerializeField] Color[] _gridColor;
    public Color[] GridColor => _gridColor;
    // Start is called before the first frame update
    void Start()
    {
        _currentStage = stageDatas[Random.Range(0, stageDatas.Count)];
        _grids = new Grid[_currentStage.Size.x, _currentStage.Size.y];
        GameObject gridParent = GameObject.Find("Grids");

        for (float y = 0, z = 0; y < _currentStage.Size.y; y++, z += Mathf.Sin(60 * Mathf.Deg2Rad))
        {
            for (int x = 0; x < _currentStage.Size.x; x++)
            {
                GameObject gridObject = Instantiate(_gridGameObject);
                //À•W”z’u
                if (y % 2 == 0) gridObject.transform.position = new Vector3(x, 0, z);
                else gridObject.transform.position = new Vector3(0.5f + x, 0, z);

                gridObject.transform.parent = gridParent.transform;
                Grid grid = gridObject.GetComponent<Grid>();
                grid.GridPos = new Vector2(x, y);
                grid.WorldPos = gridObject.transform.position;
                _grids[x, (int)y] = grid;
                gridObject.name = $"grid({x}, {y})";

                if (_gridText)
                {
                    GameObject text = Instantiate(_gridText);
                    text.transform.position = grid.WorldPos += new Vector3(0, 0.1f, 0);
                    text.GetComponent<TextMesh>().text = $"{x},{y}";
                    text.transform.parent = gridObject.transform;
                }
            }
        }

        GameObject objParent = GameObject.Find("Obj");

        for(int i = 0; i < GameManager.Instance.Players.Count ;i++)
        {
            ObjectSO obj = GameManager.Instance.Players[i];
            StageDataSO.Pos pos = _currentStage.PlayerStart[i];
            _grids[pos.x, pos.y].OnObject = obj;
            GameObject g = Instantiate(obj.StartObject);
            g.transform.position = _grids[pos.x, pos.y].WorldPos;
            obj.Object = g;
            obj.IsMove = true;
            obj.IsAttack = true;
            obj.IsAlive = true;
            obj.Hp = obj.MaxHp;
            g.transform.parent = objParent.transform;
        }

        for (int i = 0; i < GameManager.Instance.Enemys.Count; i++)
        {
            ObjectSO obj = GameManager.Instance.Enemys[i];
            StageDataSO.Pos pos = _currentStage.EnemyStart[i];
            _grids[pos.x, pos.y].OnObject = obj;
            GameObject g = Instantiate(obj.StartObject);
            g.transform.position = _grids[pos.x, pos.y].WorldPos;
            obj.Object = g;
            obj.IsMove = true;
            obj.IsAttack = true;
            obj.IsAlive = true;
            obj.Hp = obj.MaxHp;
            g.transform.parent = objParent.transform;
        }

        ObjectSO obstacle = GameManager.Instance.Obstacle;
        foreach (StageDataSO.Pos obj in Stage.ObstaclePos)
        {
            _grids[obj.x, obj.y].OnObject = obstacle;
            GameObject g = Instantiate(obstacle.StartObject);
            g.transform.position = _grids[obj.x, obj.y].WorldPos;
            g.transform.parent = objParent.transform;
        }

        _grids[Stage.Goal.x, Stage.Goal.y].State = GridState.Goal;

        Vector3 maxGrid = _grids[_currentStage.Size.x - 1, _currentStage.Size.y - 1].WorldPos;
        MouseClick.Instance.CamXmax = maxGrid.x;
        MouseClick.Instance.CamZmax = maxGrid.z;

        ObjectMove.Grid = _grids;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
