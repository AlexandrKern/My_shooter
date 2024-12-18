using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public IInputMove _currentInputMove;
    public IInputShooter _currentInputShoot;
    private void Awake()
    {
        InitializeInput();
    }
    private void InitializeInput()
    {
        if (true)
        {
            _currentInputMove = new InputKeyBorad();
            _currentInputShoot = new InputKeyBorad();
        }
    }
}
