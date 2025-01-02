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
    private float animationWeaponDuration = 2f; // Продолжительность анимации
    [Foldout("Image Weapon")]
    [SerializeField]
    private float scaleWeaponFactor = 1.5f; // Масштаб для увеличения
    [Foldout("Image Weapon")]
    [SerializeField]
    private float rotationAngle = 180f; // Угол поворота
    [Foldout("Image Weapon")]
    [SerializeField]
    private Vector3 originalScale; // Исходный масштаб
    [Foldout("Image Weapon")]
    [SerializeField]
    private Vector3 originalPosition; // Исходная позиция
    [Foldout("Image Weapon")]
    [SerializeField]
    private float originalAlpha; // Исходная прозрачность

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
        // Сохраняем исходные значения
        originalScale = image.transform.localScale;
        originalPosition = image.transform.position;
        originalAlpha = image.color.a;

        // 1. Анимация прозрачности
        var fade = image.DOFade(0.3f, animationDuration).SetEase(Ease.InOutQuad);

        // 2. Анимация масштаба
        var scale = image.transform.DOScale(scaleFactor, animationDuration).SetEase(Ease.InOutQuad);

        // 3. Анимация поворота
        var rotate = image.transform.DORotate(new Vector3(0, 0, rotationAngle), animationDuration, RotateMode.FastBeyond360).SetEase(Ease.InOutQuad);

        // 4. Восстановление всех свойств до исходных
        var sequence = DOTween.Sequence()
            .Append(fade)
            .Join(scale)
            .Join(rotate)
            .AppendInterval(1f) // Ожидание перед возвратом
            .Append(image.DOFade(originalAlpha, animationDuration))
            .Join(image.transform.DOScale(originalScale, animationDuration))
            .Join(image.transform.DORotate(Vector3.zero, animationDuration, RotateMode.FastBeyond360))
            .OnComplete(() => Debug.Log("Анимация завершена!")); // Завершение анимации

    }

}
