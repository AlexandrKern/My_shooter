using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputKeyBorad : IInputMove,IInputShooter
{
    private bool _isShooting;

    public Vector3 GetDirection()
    {
         Vector3 direction = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            direction.z = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction.z = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.x = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x = 1;
        }

        return direction.normalized;
    }

    public bool IsJumpPressed()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public bool IsNextBullet()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    public bool IsNextWepon()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }

    public bool IsRecharge()
    {
        return Input.GetKeyDown(KeyCode.R);
    }

    public bool IsShootQueue()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isShooting  = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _isShooting = false;
        }
        return _isShooting;
    }

    public bool IsShootSingle()
    {
        return Input.GetMouseButtonDown(0);
    }
}
