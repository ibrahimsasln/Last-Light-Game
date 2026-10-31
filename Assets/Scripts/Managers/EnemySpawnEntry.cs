using System;
using UnityEngine;
using LastLight.Enemies;

namespace LastLight.Managers
{
    [Serializable]
    public class EnemySpawnEntry
    {
        [SerializeField] Enemy prefab;
        [SerializeField, Min(1)] int startWave = 1;
        [SerializeField, Min(0f)] float weight = 1f;

        public Enemy Prefab => prefab;
        public float Weight => weight;

        public bool IsAvailable(int wave) => wave >= startWave;
    }
}
