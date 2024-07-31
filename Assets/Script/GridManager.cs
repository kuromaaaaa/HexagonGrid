using UnityEngine;

public class GridManager : SingletonMonoBehaviour<GridManager>
{
    [SerializeField] int _sizeX;
    [SerializeField] int _sizeY;
    [SerializeField] GameObject _gridGameObject;
    // Start is called before the first frame update
    void Start()
    {
        for (float y = 0, z = 0; y < _sizeY; y++, z += Mathf.Sin(60 * Mathf.Deg2Rad))
        {
            for (int x = 0; x < _sizeX; x++)
            {
                GameObject g = Instantiate(_gridGameObject);
                //À•W”z’u
                if (y % 2 == 0) g.transform.position = new Vector3(x, 0, z);
                else g.transform.position = new Vector3(0.5f + x, 0, z);

                g.name = $"grid({x}, {y})";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
