using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationButtonManager : MonoBehaviour
{
    [SerializeField] private float scaleMultiplier = 1.2f; 
    [SerializeField] private float duration = 0.5f; 
    public void ButtonPulsation(Button button)
    {
        button.transform
          .DOScale(scaleMultiplier, duration)
          .SetEase(Ease.InOutSine)
          .SetLoops(-1, LoopType.Yoyo);
    }

}
