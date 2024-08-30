using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    float _alpha = 1.0f;
    private void Start()
    {
        GetComponent<Image>().DOFade(0f, 1.5f).OnComplete(() => Destroy(this.gameObject));
    }
}
