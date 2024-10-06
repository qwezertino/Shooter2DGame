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