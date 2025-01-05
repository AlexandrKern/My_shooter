using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationButtonManager : MonoBehaviour
{
    #region ButtonClick
    [Foldout("Button Click")]
    [SerializeField]
    public float scaleAmount = 0.9f;
    [Foldout("Button Click")]
    [SerializeField]
    public float durationScale = 0.1f;
    private Vector3 originalScale;
    #endregion

    #region Button Puslsation
    [Foldout("Button Puslsation")]
    [SerializeField]
    private float duration = 0.5f;
    [Foldout("Button Puslsation")]
    [SerializeField]
    private float scaleMultiplier = 1.2f;
    #endregion

    public void ButtonPulsation(Button button)
    {
        button.transform
          .DOScale(scaleMultiplier, duration)
          .SetEase(Ease.InOutSine)
          .SetLoops(-1, LoopType.Yoyo);
    }

    public void ButtonChangeScale(Button button)
    {
        originalScale = button.transform.localScale;
        button.transform.DOScale(originalScale * scaleAmount, durationScale).OnComplete(() =>
        {
            button.transform.DOScale(originalScale, durationScale);
        });
    }

}
