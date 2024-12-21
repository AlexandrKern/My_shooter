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
}
