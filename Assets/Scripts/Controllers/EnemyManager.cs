using System.Collections.Generic;
using Enemy;
using UnityEngine;
using VContainer.Unity;

namespace Controllers
{
    public class EnemyManager : IStartable, ITickable
    {
        public int EnemyCount => _enemies.Count;
        public IReadOnlyCollection<EnemyController> Enemies => _enemies;
        
        private readonly List<EnemyController> _enemies = new();

        public void Start()
        {
            EnemyController[] enemies = Object.FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
            foreach (var enemy in enemies)
                RegisterEnemy(enemy);
        }

        public void RegisterEnemy(EnemyController enemy)
        {
            _enemies.Add(enemy);
            enemy.OnDeath += () => _enemies.Remove(enemy);
        }

        public void Tick()
        {
        }
    }
}