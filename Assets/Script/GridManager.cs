using UnityEngine;

public class GridManager : SingletonMonoBehaviour<GridManager>
{
    [SerializeField] int _sizeX;
    public int SizeX => _sizeX;
    [SerializeField] int _sizeY;
    public int SizeY => _sizeY;
    [SerializeField] GameObject _gridGameObject;
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
                GameObject g = Instantiate(_gridGameObject);
                //À•W”z’u
                if (y % 2 == 0) g.transform.position = new Vector3(x, 0, z);
                else g.transform.position = new Vector3(0.5f + x, 0, z);

                Grid grid = g.GetComponent<Grid>();
                grid.GridPos = new Vector2(x, y);
                grid.Pos = g.transform.position;
                _grids[x, (int)y] = grid;


                g.name = $"grid({x}, {y})";
            }
        }

        _grids[8, 8].State = GridState.Player;

        Vector3 maxGrid = _grids[_sizeX - 1, _sizeY - 1].Pos;
        MouseClick.Instance.CamXmax = maxGrid.x;
        MouseClick.Instance.CamZmax = maxGrid.z;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
