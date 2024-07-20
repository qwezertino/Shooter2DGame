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
    }
}