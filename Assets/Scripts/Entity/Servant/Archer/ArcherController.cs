using Entity.States;
using Servant;
using UnityEngine;

namespace Entity.Servant.Archer
{
    public class ArcherController : ServantController, IArcher
    {
        public ArrowData ArrowData => new ArrowData
        {
            damage = AttackDamage,
            speed = 8,
            distance = 10000,
            targetLayer = LayerMasks.Enemy | LayerMasks.NeutralEntity
        };
        
        public string ArrowPath { get; set; }
        public int AttackDamage { get; set; }
        public float AttackTimeOut { get; set; } = 2f;
        
        [SerializeField] private SpriteRenderer _body;
        [SerializeField] private SpriteRenderer _bow;
        
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void StartFirstState()
        {
            Resources.Load<GameObject>("Projectile/ArcherArrowGrade1");
        }
       
        public void SetBodySprite(Sprite body)
        {
            _body.sprite = body;
        }

        public void SetBowSprite(Sprite bow)
        {
            _bow.sprite = bow;
        }

        public void SetFootSprite(Sprite foot)
        {
            var footController = GetComponentInChildren<FootController>();
            footController.SetFootSprites(foot);
        }
    }
}