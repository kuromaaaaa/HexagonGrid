using UnityEngine;

public class GridManager : SingletonMonoBehaviour<GridManager>
{
    [SerializeField] int _sizeX;
    public int SizeX => _sizeX;
    [SerializeField] int _sizeY;
    public int SizeY => _sizeY;
    [SerializeField] GameObject _gridGameObject;
    [SerializeField] GameObject _gridText;
    Grid[,] _grids;
    public Grid[,] Grids => _grids;

    [SerializeField] Color[] _gridColor;
    public Color[] GridColor => _gridColor;
    // Start is called before the first frame update
    void Start()
    {
        _grids = new Grid[_sizeX, _sizeY];

        for (float y = 0, z = 0; y < _sizeY; y++, z += Mathf.Sin(60 * Mathf.Deg2Rad))
        {
            for (int x = 0; x < _sizeX; x++)
            {
                GameObject gridObject = Instantiate(_gridGameObject);
                //À•W”z’u
                if (y % 2 == 0) gridObject.transform.position = new Vector3(x, 0, z);
                else gridObject.transform.position = new Vector3(0.5f + x, 0, z);

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

        foreach (var data in GameManager.Instance.Objects)
        {

            _grids[(int)data.StartPosition.x, (int)data.StartPosition.y].OnObject = data.ObjectData;
            data.ObjectData.Pos = data.StartPosition;
            GameObject g = Instantiate(data.ObjectData.StartObject);
            data.ObjectData.Object = g;
            data.ObjectData.IsMove = true;
            data.ObjectData.IsAttack = true;
            data.ObjectData.IsAlive = true;
            data.ObjectData.Hp = data.ObjectData.MaxHp;
            g.transform.position = _grids[(int)data.StartPosition.x, (int)data.StartPosition.y].WorldPos;
            switch(data.ObjectData.Type)
            {
                case (ObjectType.Player):
                {
                    GameManager.Instance.Players.Add(data.ObjectData);
                    break;
                }
                case (ObjectType.Enemy):
                {
                    GameManager.Instance.Enemys.Add(data.ObjectData);
                    break;
                }
                case (ObjectType.Obstacle):
                {
                    GameManager.Instance.Obstacles.Add(data.ObjectData);
                    break;
                }
            }
        }

        Vector3 maxGrid = _grids[_sizeX - 1, _sizeY - 1].WorldPos;
        MouseClick.Instance.CamXmax = maxGrid.x;
        MouseClick.Instance.CamZmax = maxGrid.z;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
