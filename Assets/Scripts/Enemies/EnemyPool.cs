using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace LastLight.Enemies
{
    public class EnemyPool
    {
        readonly Dictionary<Enemy, ObjectPool<Enemy>> pools = new Dictionary<Enemy, ObjectPool<Enemy>>();
        readonly int capacity;
        readonly int maxSize;

        public EnemyPool(int capacity, int maxSize)
        {
            this.capacity = capacity;
            this.maxSize = maxSize;
        }

        public Enemy Get(Enemy prefab, Vector2 position)
        {
            if (!pools.TryGetValue(prefab, out ObjectPool<Enemy> pool))
            {
                pool = CreatePool(prefab);
                pools.Add(prefab, pool);
            }

            Enemy enemy = pool.Get();
            enemy.transform.position = position;
            return enemy;
        }

        private ObjectPool<Enemy> CreatePool(Enemy prefab)
        {
            ObjectPool<Enemy> pool = null;
            pool = new ObjectPool<Enemy>(
                createFunc: () =>
                {
                    Enemy enemy = Object.Instantiate(prefab);
                    enemy.SetPool(pool);
                    return enemy;
                },
                actionOnGet: enemy => enemy.gameObject.SetActive(true),
                actionOnRelease: enemy => enemy.gameObject.SetActive(false),
                actionOnDestroy: enemy => Object.Destroy(enemy.gameObject),
                defaultCapacity: capacity,
                maxSize: maxSize);
            return pool;
        }
    }
}
