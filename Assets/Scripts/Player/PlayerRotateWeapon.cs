using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActionGame.Utils;

public class PlayerRotateWeapon : MonoBehaviour
{
    private void Update()
    {
        HandleAiming();
    }
    private void HandleAiming()
    {
        Vector3 mousePosition = GameInput.Instance.GetMouseWorldPosition();
        float angle = Utils.RotateTowardsMouse(transform, mousePosition);

        Vector3 localScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            localScale.y = -1f;
        }
        else
        {
            localScale.y = 1f;
        }
        transform.localScale = localScale;
    }
}
