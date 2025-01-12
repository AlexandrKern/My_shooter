using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputKeyBorad : IInputMove, IInputShooter
{
    private bool _isShooting;

    public Vector3 GetDirection()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        return new Vector3(horizontal, 0, vertical);
    }

    public Vector3 GetMousePosition()
    {
        return Input.mousePosition;
    }

    public bool IsJumpPressed() => Input.GetKeyDown(KeyCode.Space);
    public bool IsNextBullet() => Input.GetKeyDown(KeyCode.E);
    public bool IsNextWepon() => Input.GetKeyDown(KeyCode.Q);
    public bool IsRecharge() => Input.GetKeyDown(KeyCode.R);

    public bool IsShootQueue()
    {
        if (Input.GetMouseButtonDown(0)) _isShooting = true;
        if (Input.GetMouseButtonUp(0)) _isShooting = false;
        return _isShooting;
    }

    public bool IsShootSingle() => Input.GetMouseButtonDown(0);
}

