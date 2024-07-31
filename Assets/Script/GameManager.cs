using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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

    void SelectGridChange(Grid value)
    {
        if(SelectGrid)
            SelectGrid.State = GridState.None;
        value.State = GridState.Select;

        Camera.main.gameObject.transform.
            DOMove(new Vector3(value.Pos.x, _cm.position.y, value.Pos.z),0.5f)
            .SetEase(Ease.Linear);

        switch(gameState)
        {
            case (GameState.None):
            {
                if(value.OnObject)
                    gameState = GameState.SelectObject;
                break;
            }
        }
    }
}
