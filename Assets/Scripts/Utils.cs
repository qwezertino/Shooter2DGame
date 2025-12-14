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