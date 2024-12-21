using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControler : MonoBehaviour
{
    #region Singleton
    public static GameControler Instace;

    private void Awake()
    {
        if (Instace == null)
        {
            Instace = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion


    public InputManager inputManager;
    public WeponManager weponManager;
    public BulletManager bulletManager;
    public AnimationImageManager imageManager;
    public UiManager uiManager;

}
