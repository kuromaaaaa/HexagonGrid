using UnityEngine;
using UnityEngine.UIElements;

public class MouseClick : SingletonMonoBehaviour<MouseClick>
{
    RaycastHit _rayhit;
    [SerializeField] float mouseSensi = 0.3f;
    float _camXmax;
    public float CamXmax { set { _camXmax = value; } }
    float _camZmax;
    public float CamZmax { set { _camZmax = value; } }

    GameObject _camera;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _rayhit))
            {
                Debug.Log(_rayhit.collider.gameObject.GetComponent<Grid>().GridPos);
                GameManager.Instance.SelectGrid = _rayhit.collider.GetComponent<Grid>();
            }
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            _camera.transform.position += new Vector3(0, Input.mouseScrollDelta.y, 0);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 mouseMove = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")) * -1 * mouseSensi;
            _camera.transform.position += CapAtCameraPos(mouseMove);
        }
    }

    Vector3 CapAtCameraPos(Vector3 move)
    {
        Vector3 pos = Camera.main.transform.position;
        if ((pos.x < 0 && move.x < 0) || (pos.x > _camXmax && move.x > 0)) move.x = 0;
        if ((pos.z < 0 && move.z < 0) || (pos.z > _camZmax && move.z > 0)) move.z = 0;
        return move;
    }
}
