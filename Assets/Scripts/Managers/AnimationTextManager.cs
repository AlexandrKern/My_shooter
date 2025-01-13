using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimationTextManager : MonoBehaviour
{
    #region Text timer settings
    [Foldout("Text timer")]
    [SerializeField]
    float timerScaleFactor;
    [Foldout("Text timer")]
    [SerializeField]
    float timerDuration;
    #endregion

    #region Text wave count settings
    [Foldout("Wave count")]
    [SerializeField]
    float waeveCountScaleFactor;
    [Foldout("Wave count")]
    [SerializeField]
    float waveCountDuration;
    [Foldout("Wave count")]
    [SerializeField]
    Color waveCountColor;

    [Foldout("Wave count")]
    [SerializeField]
    float shakeStrength = 10f;  
    [Foldout("Wave count")]
    [SerializeField]
    int shakeVibrato = 20;    
    [Foldout("Wave count")]
    [SerializeField]
    float shakeRandomness = 90f; 
    #endregion

    private void AnimateScale(RectTransform textTransform, Vector3 originalScale, float scaleFactor, float duration)
    {
        textTransform.DOScale(originalScale * scaleFactor, duration / 2)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                textTransform.DOScale(originalScale, duration / 2)
                    .SetEase(Ease.InOutBack);
            });
    }

    public void AnimateTimerText(Text text)
    {
        RectTransform textTransform = text.rectTransform;

        Vector3 originalScale = textTransform.localScale;

        AnimateScale(textTransform, originalScale, timerScaleFactor, timerDuration);
    }

    public void AnimateWaveCountText(Text text)
    {
        RectTransform textTransform = text.rectTransform;

        Vector3 originalScale = textTransform.localScale;
        Vector3 originalPosition = textTransform.localPosition;
        Color originalColor = text.color; 

        AnimateScale(textTransform, originalScale, waeveCountScaleFactor, waveCountDuration);

        text.DOColor(waveCountColor, waveCountDuration) 
            .SetLoops(2, LoopType.Yoyo) 
            .OnKill(() => text.color = originalColor); 

        textTransform.DOShakePosition(waveCountDuration, strength: shakeStrength, vibrato: shakeVibrato, randomness: shakeRandomness)
            .SetEase(Ease.InOutQuad)
            .OnKill(() =>
            {
                textTransform.localPosition = originalPosition;
            });
    }
}

