using UnityEngine;

namespace KnightAdventure.Utilities
{
    public static class Utilities
    {
        public static Vector3 GetRandomDir()
        {             // поиск случайной точки в пространстве по осям X,Y от -1 до 1
            return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
    }
}