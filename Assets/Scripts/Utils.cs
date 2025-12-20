using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ActionGame.Utils
{
    public static class Utils
    {
        public static Vector3 GetRandomDir()
        {
            return new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        }

        public static Vector3 GetVectorFromAngle(float angle)
        {
            float angleRad = angle * (Mathf.PI / 180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }
        public static float GetAngleFromVectorFloat(Vector3 dir)
        {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            return n;
        }
        public static void RaycastLogHits(RaycastHit2D[] hits, int hitCount, string weaponName = "Weapon")
        {
            if (hitCount > 0)
            {
                Debug.Log($"<color=yellow>[{weaponName}] Raycast обнаружил {hitCount} попадани(й/я):</color>");
                for (int i = 0; i < hitCount; i++)
                {
                    string layerName = LayerMask.LayerToName(hits[i].collider.gameObject.layer);
                    string triggerInfo = hits[i].collider.isTrigger ? "<color=red>TRIGGER</color>" : "<color=green>SOLID</color>";
                    Debug.Log($"  #{i + 1}: <b>{hits[i].collider.gameObject.name}</b> | Layer: {layerName} | {triggerInfo} | Distance: {hits[i].distance:F2}");
                }
            }
        }
        public static void RaycastDrawLine(Vector3 origin, Vector3 direction, float distance, bool hasHit, Vector3 hitPoint)
        {
            Color color = hasHit ? Color.red : Color.green;
            Vector3 endPoint = hasHit ? hitPoint : origin + direction * distance;

            Debug.DrawLine(origin, endPoint, color, 0.5f);

            if (hasHit)
            {
                // Рисуем крестик в точке попадания
                Vector3 perpendicular = Vector3.Cross(direction, Vector3.forward).normalized * 0.2f;
                Debug.DrawLine(hitPoint - perpendicular, hitPoint + perpendicular, Color.yellow, 0.5f);
                Debug.DrawLine(hitPoint - Vector3.up * 0.2f, hitPoint + Vector3.up * 0.2f, Color.yellow, 0.5f);
            }
        }

        // === FOV Utilities ===

        /// <summary>
        /// Вращает transform к позиции мыши в 2D
        /// </summary>
        /// <returns>Угол поворота в градусах</returns>
        public static float RotateTowardsMouse(Transform targetTransform, Vector3 mouseWorldPosition)
        {
            Vector3 rotation = mouseWorldPosition - targetTransform.position;
            float angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            targetTransform.rotation = Quaternion.Euler(0, 0, angle);
            return angle;
        }

        /// <summary>
        /// Проверяет находится ли цель в конусе видимости
        /// </summary>
        public static bool IsInFieldOfView(Vector3 origin, Vector3 lookDirection, Vector3 targetPosition, float fovAngle)
        {
            Vector3 directionToTarget = (targetPosition - origin).normalized;
            float angleToTarget = Vector3.Angle(lookDirection, directionToTarget);
            return angleToTarget <= fovAngle / 2f;
        }

        /// <summary>
        /// Проверяет заблокирован ли обзор препятствиями через Raycast
        /// </summary>
        public static bool IsBlockedByObstacle(Vector3 origin, Vector3 targetPosition, LayerMask obstacleLayer)
        {
            Vector3 directionToTarget = targetPosition - origin;
            float distanceToTarget = directionToTarget.magnitude;

            RaycastHit2D hit = Physics2D.Raycast(origin, directionToTarget.normalized, distanceToTarget, obstacleLayer);

            return hit.collider != null;
        }

        /// <summary>
        /// Рисует круг через Gizmos
        /// </summary>
        public static void DrawGizmosCircle(Vector3 center, float radius, int segments = 50)
        {
            float angleStep = 360f / segments;
            Vector3 previousPoint = center + new Vector3(radius, 0, 0);

            for (int i = 1; i <= segments; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector3 newPoint = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
                Gizmos.DrawLine(previousPoint, newPoint);
                previousPoint = newPoint;
            }
        }

        /// <summary>
        /// Рисует конус FOV через Gizmos
        /// </summary>
        public static void DrawGizmosFOVCone(Vector3 origin, Vector3 direction, float fovAngle, float distance, int arcSegments = 20)
        {
            float halfAngle = fovAngle / 2f;

            // Левая граница конуса
            Vector3 leftBoundary = Quaternion.Euler(0, 0, halfAngle) * direction * distance;
            Gizmos.DrawLine(origin, origin + leftBoundary);

            // Правая граница конуса
            Vector3 rightBoundary = Quaternion.Euler(0, 0, -halfAngle) * direction * distance;
            Gizmos.DrawLine(origin, origin + rightBoundary);

            // Дуга конуса
            Vector3 previousPoint = origin + leftBoundary;
            for (int i = 1; i <= arcSegments; i++)
            {
                float currentAngle = halfAngle - (fovAngle * i / arcSegments);
                Vector3 newPoint = origin + Quaternion.Euler(0, 0, currentAngle) * direction * distance;
                Gizmos.DrawLine(previousPoint, newPoint);
                previousPoint = newPoint;
            }
        }

        // public static Vector3 GetMouseWorldPosition() {
        //     Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        //     vec.z = 0f;
        //     return vec;
        // }

        // public static Vector3 GetMouseWorldPositionWithZ() {
        //     return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        // }

        // public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera) {
        //     return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        // }

        // public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
        //     Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        //     return worldPosition;
        // }
    }
}