using Controllers;
using UnityEngine;
using VContainer;

namespace Level.Tutorial
{
    public class TutorialCheckKillAllEnemies : MonoBehaviour
    {
        [SerializeField] private MagicBarrier _magicBarrier;
        [Inject] private EnemyManager _enemyManager;
        private bool _allEnemiesKilled;

        public void Update()
        {
            if (_enemyManager.Enemies.Count > 0 || _allEnemiesKilled) return;
            _allEnemiesKilled = true;
            _magicBarrier.Off();
        }
    }
}