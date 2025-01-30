using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class AnimationObjectsManager : MonoBehaviour
{
    #region object
    [Foldout("object")]
    [SerializeField]
    private float objectScaleMultiplier = 1.2f; 
    [Foldout("object")]
    [SerializeField] private float objectScaleDuration = 0.5f;   
    [Foldout("object")]
    [SerializeField]
    private float objectRotationAngle = 15f;   
    [Foldout("object")]
    [SerializeField] private float objectRotationDuration = 1f;  
    #endregion

    public void AnimateObject(Transform transform)
    {
        
        transform.DOScale(transform.localScale * objectScaleMultiplier, objectScaleDuration)
            .SetLoops(-1, LoopType.Yoyo) 
            .SetEase(Ease.InOutSine);

        
        transform.DORotate(new Vector3(0, objectRotationAngle, 0), objectRotationDuration, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Yoyo) 
            .SetEase(Ease.InOutSine);
    }
}
