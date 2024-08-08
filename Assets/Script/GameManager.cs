using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    GameState gameState;
    Grid _selectGrid;
    public Grid SelectGrid
    {
        get { return _selectGrid; }
        set 
        { 
            SelectGridChange( _selectGrid,value);
            _selectGrid = value;
        }
    }

    Transform _cm;

    [SerializeField] List<ObjectSO> objects;
    public List<ObjectSO> Objects { get { return objects; } }

    private void Start()
    {
        _cm = Camera.main.transform;
    }

    void SelectGridChange(Grid beforeSelect , Grid selectGrid)
    {
        Camera.main.gameObject.transform.
            DOMove(new Vector3(selectGrid.WorldPos.x, _cm.position.y, selectGrid.WorldPos.z),0.5f)
            .SetEase(Ease.Linear);

        switch(gameState)
        {
            case (GameState.None):
            {
                if (selectGrid.OnObject)
                {
                    gameState = GameState.SelectObject;
                    if (selectGrid.State == GridState.Player)
                        ObjectMove.MoveRange((int)selectGrid.GridPos.x,(int)selectGrid.GridPos.y,selectGrid.OnObject.Step);
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
                if(selectGrid.State == GridState.OnMove)
                {
                    int bx = (int)beforeSelect.GridPos.x;
                    int by = (int)beforeSelect.GridPos.y;
                    int sx = (int)selectGrid.GridPos.x;
                    int sy = (int)selectGrid.GridPos.y;
                    ObjectMove.AStarSearch(bx,by,sx,sy);
                }
                break;
            }
        }
    }
}
