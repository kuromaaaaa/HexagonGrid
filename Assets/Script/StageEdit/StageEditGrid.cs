using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageEditGrid : SingletonMonoBehaviour<StageEditGrid>
{
    [SerializeField] StageDataSO _editStage;
    [SerializeField] GameObject _gridGameObject;
    [SerializeField] GameObject _gridText;
    EditGrid[,] _grids;
    [SerializeField] Text _selectGridText;
    [SerializeField] Text _typeText;

    List<StageDataSO.Pos> allObj;

    EditGritType _type = EditGritType.None;
    public EditGritType Type 
    {
        get => _type;
        set 
        {
            if ((int)value == -1)
                value = (EditGritType)4;
            _type = (EditGritType)((int)value % 5);
            _typeText.text = $"{_type}";
        }
    }
    EditGrid _select;
    public GameObject Select 
    { 
        set 
        {
            foreach (EditGrid g in _grids) 
            { 
                if(g.obj == value)
                {
                    _select = g;
                    _selectGridText.text = $"{g.pos.x} , {g.pos.y}";
                    break;
                }
            }

            SelectGridReset();
            switch(_type)
            {
                case EditGritType.PlayerStart:
                {
                    _editStage.PlayerStart.Add(_select.pos);
                    break;
                }
                case EditGritType.EnemyStart:
                {
                    _editStage.EnemyStart.Add(_select.pos);
                    break;
                }
                case EditGritType.Obstacle:
                {
                    _editStage.ObstaclePos.Add(_select.pos);
                    break; 
                }
                case EditGritType.Goal: 
                {
                    _editStage.Goal = _select.pos;
                    break;
                }
            }
        } 
    }
    void Start()
    {
        _grids = new EditGrid[_editStage.Size.x, _editStage.Size.y];

        for (float y = 0, z = 0; y < _editStage.Size.y; y++, z += Mathf.Sin(60 * Mathf.Deg2Rad))
        {
            for (int x = 0; x < _editStage.Size.x; x++)
            {
                GameObject gridObject = Instantiate(_gridGameObject);
                //座標配置
                if (y % 2 == 0) gridObject.transform.position = new Vector3(x, 0, z);
                else gridObject.transform.position = new Vector3(0.5f + x, 0, z);

                EditGrid grid = new EditGrid();
                grid.pos = new StageDataSO.Pos(x, (int)y);
                grid.obj = gridObject;
                _grids[x, (int)y] = grid;
                gridObject.name = $"grid({x}, {y})";

                if (_gridText)
                {
                    GameObject text = Instantiate(_gridText);
                    text.transform.position = gridObject.transform.position += new Vector3(0, 0.1f, 0);
                    text.GetComponent<TextMesh>().text = $"{x},{y}";
                    text.transform.parent = gridObject.transform;
                }
            }
        }
        //プレイヤースタートの設置
        foreach (StageDataSO.Pos xy in _editStage.PlayerStart)
        {
            _grids[xy.x, xy.y].obj.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        //エネミースタートの設置
        foreach(StageDataSO.Pos xy in _editStage.EnemyStart)
        {
            _grids[xy.x, xy.y].obj.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        //障害物の設置
        foreach(StageDataSO.Pos xy in _editStage.ObstaclePos) 
        {
            _grids[xy.x, xy.y].obj.GetComponent<MeshRenderer>().material.color = Color.gray;
        }
    }

    private void SelectGridReset()
    {
        foreach (StageDataSO.Pos xy in _editStage.PlayerStart)
        {
            if (xy.x == _select.pos.x && xy.y == _select.pos.y)
            {
                _editStage.PlayerStart.Remove(xy);
                return;
            }
        }
        foreach (StageDataSO.Pos xy in _editStage.EnemyStart)
        {
            if (xy.x == _select.pos.x && xy.y == _select.pos.y)
            {
                _editStage.EnemyStart.Remove(xy);
                return;
            }
        }
        foreach (StageDataSO.Pos xy in _editStage.ObstaclePos)
        {
            if (xy.x == _select.pos.x && xy.y == _select.pos.y)
            {
                _editStage.ObstaclePos.Remove(xy);
                return;
            }
        }
    }

    public class EditGrid
    {
        public StageDataSO.Pos pos;
        public GameObject obj;
    }

    public enum EditGritType
    {
        None,
        PlayerStart,
        EnemyStart,
        Obstacle,
        Goal
    }
}
