using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseClick : MonoBehaviour
{
    RaycastHit _rayhit;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out _rayhit))
            {
                Debug.Log(_rayhit.collider.name);
                GameManager.Instance.SelectGrid = _rayhit.collider.GetComponent<Grid>();
            }
        }
        if(Input.mouseScrollDelta.y != 0)
        {
            Camera.main.transform.position += new Vector3(0,Input.mouseScrollDelta.y,0);
        }
    }
}
