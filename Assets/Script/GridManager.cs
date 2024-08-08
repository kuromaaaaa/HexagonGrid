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

                if(_gridText)
                {
                    GameObject text = Instantiate(_gridText);
                    text.transform.position = grid.WorldPos += new Vector3(0,0.1f,0);
                    text.GetComponent<TextMesh>().text = $"{x},{y}";
                }
            }
        }

        foreach(var data in GameManager.Instance.Objects)
        {
            switch(data.Type)
            {
                case (ObjectType.Player):
                {
                    _grids[(int)data.Pos.x, (int)data.Pos.y].OnObject = data;
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
