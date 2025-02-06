using BaseEntity.States;
using Servant.FSM;
using UnityEngine;
using VContainer;

namespace Servant.Archer
{
    public class ArcherController : ServantController, IArcher
    {
        public ArrowData ArrowData => new ArrowData
        {
            damage = AttackDamage,
            speed = 8,
            distance = 10000,
            targetLayer = LayerMask.GetMask("Enemy")
        };
        
        public string ArrowPath { get; set; }
        public int AttackDamage { get; set; }

        public float AttackTimeOut { get; set; } = 2f;
        
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void StartFirstState()
        {
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