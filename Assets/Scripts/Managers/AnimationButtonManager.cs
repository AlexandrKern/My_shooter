using DG.Tweening;
using NaughtyAttributes;
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

    #region Button Pulsation
    [Foldout("Button Pulsation")]
    [SerializeField]
    private float duration = 0.5f;
    [Foldout("Button Pulsation")]
    [SerializeField]
    private float scaleMultiplier = 1.2f;
    #endregion

    #region Button Shake
    [Foldout("Button Shake")]
    [SerializeField]
    private float shakeDuration = 0.5f;
    [Foldout("Button Shake")]
    [SerializeField]
    private float shakeStrength = 10f;
    [Foldout("Button Shake")]
    [SerializeField]
    private int shakeVibrato = 10;

    private Tween shakeTween;
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

    public void StartButtonShake(Button button)
    {
        StopButtonShake();

        shakeTween = button.transform.DOShakePosition(
            shakeDuration,
            shakeStrength,
            shakeVibrato,
            randomness: 90,
            snapping: false,
            fadeOut: false
        ).SetLoops(-1);
    }

    public void StopButtonShake()
    {
        if (shakeTween != null && shakeTween.IsActive())
        {
            shakeTween.Kill();
            shakeTween = null;
        }
    }

}
