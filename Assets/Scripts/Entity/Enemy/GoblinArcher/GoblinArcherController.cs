using Entity.States;
using UnityEngine;

namespace Entity.Enemy.GoblinArcher
{
    public class GoblinArcherController : EnemyController, IArcher
    {
        public float AttackTimeOut { get; set; } = 1f;
        public ArrowData ArrowData => new ArrowData
        {
            damage = AttackDamage,
            speed = 8,
            distance = 10000,
            targetLayer = LayerMasks.King | LayerMasks.NeutralEntity
        };
        
        public string ArrowPath { get => ArrowPrefPath; set => ArrowPrefPath = value; }
        public EntityController Controller => this;
        public int AttackDamage => attackDamage;

        [SerializeField] private int attackDamage;
        public string ArrowPrefPath;
    }
}