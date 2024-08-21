using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageEditMouseMove : MonoBehaviour
{
    RaycastHit _rayhit;
    EventSystem _es;
    void Start()
    {
        _es = EventSystem.current;
    }
    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Vector3 mouseMove = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")) * -1 * 0.5f;
            Camera.main.transform.position += mouseMove;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _rayhit) &&
                    !_es.IsPointerOverGameObject())
            {
                if (!_rayhit.collider.gameObject.TryGetComponent<Grid>(out Grid grid))
                {
                    Debug.LogError("エラー：たぶんコライダーがついてる");
                }
                StageEditGrid.Instance.Select = _rayhit.collider.gameObject;
            }
        }

        if(Input.mouseScrollDelta.y != 0)
        {
            StageEditGrid.Instance.Type += (int)Input.mouseScrollDelta.y;
        }
    }
}
