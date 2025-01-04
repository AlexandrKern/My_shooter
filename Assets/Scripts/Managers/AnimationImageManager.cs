using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationImageManager : MonoBehaviour
{
    #region Image division settings
    [Foldout("Image division")]
    [SerializeField]
    private float animationDuration = 1f;
    [Foldout("Image division")]
    [SerializeField]
    private Vector3 scaleFactor = new Vector3(1.2f, 1f, 1f);
    [Foldout("Image division")]
    [SerializeField]
    private float startAlpha = 0f;
    [Foldout("Image division")]
    [SerializeField]
    private float endAlpha = 1f;
    [Foldout("Image division")]
    [SerializeField]
    private Color startColor = Color.white;
    [Foldout("Image division")]
    [SerializeField]
    private Color endColor = Color.green;
    [Foldout("Image division")]
    [SerializeField]
    private Vector3 moveOffset = new Vector3(0f, 50f, 0f);
    #endregion

    #region Image Notification
    [Foldout("Image notification")]
    [SerializeField]
    private float showDuration = 2f;
    #endregion

    #region Image Weapon
    [Foldout("Image Weapon")]
    [SerializeField] 
    private float pulseScale = 1.2f;   // Максимальный размер во время пульсации
    [Foldout("Image Weapon")]
    [SerializeField] 
    private float duration = 0.5f;     // Длительность анимации


    #endregion
    public void AnimationImageDivision(Image image)
    {
        Image division = image;

        Vector3 initialPosition = division.transform.localPosition;

        division.color = new Color(division.color.r, division.color.g, division.color.b, startAlpha); 
        division.transform.localScale = Vector3.one; 

        Sequence sequence = DOTween.Sequence();

        sequence.Append(division.DOFade(endAlpha, animationDuration / 2).SetEase(Ease.InOutQuad));

        sequence.Join(division.transform.DOScale(scaleFactor, animationDuration / 2).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBack));

        sequence.Join(division.transform.DOLocalMove(division.transform.localPosition + moveOffset, animationDuration / 2).SetEase(Ease.OutQuad));

        sequence.Join(division.transform.DORotate(new Vector3(0, 0, 360), animationDuration, RotateMode.FastBeyond360).SetEase(Ease.OutQuad));

        sequence.Join(division.DOColor(endColor, animationDuration / 2).SetEase(Ease.InOutSine));

        sequence.Append(division.transform.DOScale(Vector3.one, animationDuration / 2).SetEase(Ease.InOutQuad));

        sequence.Append(division.transform.DOLocalMove(initialPosition, animationDuration / 2).SetEase(Ease.InOutQuad));

        sequence.Play();
    }

    public void AnimationNotification(Image image,Text text)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);

        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(image.DOFade(0.3f, showDuration))
            .Join(text.DOFade(0.3f, showDuration))
            .AppendInterval(showDuration) 
            .Append(image.DOFade(0, showDuration))
            .Join(text.DOFade(0, showDuration))
            .Play();
    }
    public void AnimationWeapon(Image image)
    {
        RectTransform targetTransform = image.rectTransform;
        Vector3 originalScale = targetTransform.localScale;

        // Анимация: увеличение и возврат к оригинальному масштабу
        targetTransform
            .DOScale(originalScale * pulseScale, duration / 2)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
                targetTransform
                    .DOScale(originalScale, duration / 2)
                    .SetEase(Ease.InQuad)
            );
    }

}
