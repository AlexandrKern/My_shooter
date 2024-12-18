using UnityEngine;

public interface IInputMove
{
    Vector3 GetDirection();
    bool IsJumpPressed();
}
