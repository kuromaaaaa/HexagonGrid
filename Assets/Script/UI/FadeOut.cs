using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    Action _gameClear;
    public Action GameClear {
        get => _gameClear;
        set { _gameClear = value; }
    }
    private void OnEnable()
    {
        GetComponent<Image>().DOFade(1f, 1.5f).OnComplete(() => _gameClear());
    }
}
