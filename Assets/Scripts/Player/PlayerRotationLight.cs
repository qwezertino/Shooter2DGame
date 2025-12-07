using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationLight : MonoBehaviour
{
    private void Update()
    {
        HandleRotation();
    }
    private void HandleRotation()
    {
        Vector3 mousePosition = GameInput.Instance.GetMouseWorldPosition();
        Vector3 rotation = mousePosition - transform.position;
        float angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
