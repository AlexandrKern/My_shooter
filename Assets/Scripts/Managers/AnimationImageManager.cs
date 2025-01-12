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
    [Foldout("Image notification")]
    [SerializeField]
    [Range(0f, 1f)]
    private float imageAlpha = 1f;
    #endregion

    #region Image Weapon
    [Foldout("Image Weapon")]
    [SerializeField] 
    private float pulseScale = 1.2f;   // Максимальный размер во время пульсации
    [Foldout("Image Weapon")]
    [SerializeField] 
    private float duration = 0.5f;     // Длительность анимации
    private bool isAnimating = false;


    #endregion

    #region Image Health bar
    [Foldout("Image Health bar")]
    [SerializeField]
    private float animationDurationHealthBar = 0.5f;
    [Foldout("Image Health bar")]
    [SerializeField] 
    private Color fullHealthColor = Color.green;
    [Foldout("Image Health bar")]
    [SerializeField] 
    private Color lowHealthColor = Color.red;
    [Foldout("Image Health bar")]
    [SerializeField] 
    private float shakeIntensity = 10f; 
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
        Debug.Log("вызвалась анимашка");
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);

        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(image.DOFade(imageAlpha, showDuration))
            .Join(text.DOFade(imageAlpha, showDuration))
            .AppendInterval(showDuration) 
            .Append(image.DOFade(0, showDuration))
            .Join(text.DOFade(0, showDuration))
            .Play();
    }
    public void AnimationWeapon(Image image)
    {
        if (isAnimating) return;  // Проверяем, идет ли анимация

        isAnimating = true;  // Устанавливаем флаг, что анимация началась
        RectTransform targetTransform = image.rectTransform;
        Vector3 originalScale = targetTransform.localScale;

        // Анимация: увеличение и возврат к оригинальному масштабу
        targetTransform
            .DOScale(originalScale * pulseScale, duration / 2)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                targetTransform
                    .DOScale(originalScale, duration / 2)
                    .SetEase(Ease.InQuad)
                    .OnComplete(() => isAnimating = false);  // Сбрасываем флаг после завершения анимации
            });
    }

    public void AnimationUpdateHealth(float newHealthValue,Image healthImage)
    {
        if(healthImage == null) return;
        newHealthValue = Mathf.Clamp01(newHealthValue);

        healthImage.DOFillAmount(newHealthValue, animationDurationHealthBar)
            .SetEase(Ease.OutCubic)
            .OnUpdate(() => UpdateHealthBarColor(newHealthValue,healthImage)); 


        healthImage.transform.DOScale(Vector3.one * 1.1f, 0.2f)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);

        if (newHealthValue < 0.2f)
        {
            healthImage.transform.DOShakePosition(0.3f, shakeIntensity, 10, 90, false, true);
        }
    }

    private void UpdateHealthBarColor(float health, Image healthImage)
    {

        Color currentColor = Color.Lerp(lowHealthColor, fullHealthColor, health);
        healthImage.color = currentColor;
    }

}
