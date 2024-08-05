using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    GameState gameState;
    Grid _selectGrid;
    public Grid SelectGrid
    {
        get { return _selectGrid; }
        set 
        { 
            SelectGridChange(value);
            _selectGrid = value;
        }
    }

    Transform _cm;

    private void Start()
    {
        _cm = Camera.main.transform;
    }

    void SelectGridChange(Grid selectGrid)
    {
        Camera.main.gameObject.transform.
            DOMove(new Vector3(selectGrid.Pos.x, _cm.position.y, selectGrid.Pos.z),0.5f)
            .SetEase(Ease.Linear);

        switch(gameState)
        {
            case (GameState.None):
            {
                if (selectGrid.OnObject)
                {
                    gameState = GameState.SelectObject;
                    if (selectGrid.State == GridState.Player)
                        ObjectMove.MoveRange((int)selectGrid.GridPos.x,(int)selectGrid.GridPos.y,3);
                }
                break;
            }
            case (GameState.SelectObject):
            {
                if (selectGrid.State == GridState.None)
                {
                    gameState = GameState.None;
                    ObjectMove.OnMoveClear();
                }
                break;
            }
        }
    }
}
