using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActionGame.Utils;

public class PlayerRotationLight : MonoBehaviour
{
    private void Update()
    {
        HandleRotation();
    }
    private void HandleRotation()
    {
        Vector3 mousePosition = GameInput.Instance.GetMouseWorldPosition();
        Utils.RotateTowardsMouse(transform, mousePosition);
    }
}
