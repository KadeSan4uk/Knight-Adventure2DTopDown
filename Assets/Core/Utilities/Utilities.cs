using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KnightAdventure.Utilities
{
    public static class Utilities
    {
        public static Vector3 GetRandomDir()
        {
            return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
    }
}