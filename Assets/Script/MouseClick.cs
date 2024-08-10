using UnityEngine;

public class MouseClick : SingletonMonoBehaviour<MouseClick>
{
    RaycastHit _rayhit;
    [SerializeField] float mouseSensi = 0.3f;
    float _camXmax;
    public float CamXmax { set { _camXmax = value; } }
    float _camZmax;
    public float CamZmax { set { _camZmax = value; } }

    GameObject _camera;

    void Start()
    {
        _camera = Camera.main.gameObject;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _rayhit))
            {
                Grid select = _rayhit.collider.gameObject.GetComponent<Grid>();
                if (select.OnObject)
                    Debug.Log($"{select.GridPos}\n{select.WorldPos}\n{select.OnObject.ObjectName}");
                else Debug.Log($"{select.GridPos}\n{select.WorldPos}");
                GameManager.Instance.SelectGrid = select;
            }
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            _camera.transform.position += CapAtCameraPosY(Input.mouseScrollDelta.y);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 mouseMove = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")) * -1 * mouseSensi;
            _camera.transform.position += CapAtCameraPosXZ(mouseMove);
        }
        CameraPosCheck();
    }

    Vector3 CapAtCameraPosXZ(Vector3 move)
    {
        Vector3 pos = _camera.transform.position;
        if ((pos.x < 0 && move.x < 0) || (pos.x > _camXmax && move.x > 0)) move.x = 0;
        if ((pos.z < 0 && move.z < 0) || (pos.z > _camZmax && move.z > 0)) move.z = 0;
        return move;
    }

    Vector3 CapAtCameraPosY(float delta)
    {
        Debug.Log(delta);
        float pos = _camera.transform.position.y;
        if((pos > 10 && delta < 0) || (pos < 3 && delta > 0)) delta = 0;
        return new Vector3(0, delta * -1, 0);
    }

    void CameraPosCheck()
    {
        Vector3 pos = _camera.transform.position;
        if (pos.x < 0) _camera.transform.position = new Vector3(0, pos.y, pos.z);
        if (pos.z < 0) _camera.transform.position = new Vector3(pos.x, pos.y, 0);
        if (pos.x > _camXmax) _camera.transform.position = new Vector3(_camXmax, pos.y, pos.z);
        if (pos.z > _camZmax) _camera.transform.position = new Vector3(pos.x, pos.y, _camZmax);
    }
}
