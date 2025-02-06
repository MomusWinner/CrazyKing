using BaseEntity;
using BaseEntity.States;
using UnityEngine;

namespace Enemy.GoblinArcher
{
    public class GoblinArcherController : EnemyController, IArcher
    {
        public float AttackTimeOut { get; set; } = 1f;
        public ArrowData ArrowData => new ArrowData
        {
            damage = AttackDamage,
            speed = 8,
            distance = 10000,
            targetLayer = LayerMask.GetMask("King")            
        };
        
        public string ArrowPath { get => ArrowPrefPath; set => ArrowPrefPath = value; }
        public EntityController Controller => this;
        public int AttackDamage => attackDamage;

        [SerializeField] private int attackDamage;
        public string ArrowPrefPath;
        
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
}