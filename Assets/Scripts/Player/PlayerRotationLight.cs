using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationLight : MonoBehaviour
{
    [SerializeField] private float viewAngle =  75.0f;
    [SerializeField] private GameObject enemiesContainer;
    List<Transform> enemies = new List<Transform>();
    private void Start()
    {
        // Get all child Transforms of the enemiesContainer
        Transform[] enemyTransforms = enemiesContainer.GetComponentsInChildren<Transform>();

        // Loop through the transforms and print their positions
        foreach (Transform enemyTransform in enemyTransforms)
        {
            // Exclude the parent container itself
            if (enemyTransform != enemiesContainer.transform)
            {
                enemies.Add(enemyTransform);
                // Debug.Log("Enemy Position: " + enemyTransform.position);
            }
        }
    }
    private void Update()
    {
        foreach (Transform enemy in enemies)
        {
            if (!CheckInRange(new Vector2(enemy.position.x, enemy.position.y)))
            {
                return;
            }
        }
        HandleRotation();
    }
    private void HandleRotation()
    {
        Vector3 mousePosition = GameInput.Instance.GetMouseWorldPosition();
        Vector3 rotation = mousePosition - transform.position;
        float angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

    }
    public bool CheckInRange(in Vector2 position)
    {
        Vector2 dir = position - new Vector2(transform.position.x, transform.position.y);
        float dot = Vector2.Dot(new Vector2(transform.forward.x, transform.forward.y), dir.normalized);
        if (dot < Mathf.Cos(viewAngle * 0.5f * Mathf.Deg2Rad))
        {
            return false;
        }
        return true;
    }
}
