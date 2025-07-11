﻿using System;
using Entity.States;
using Servant;
using UnityEngine;

namespace Entity.Servant.Knight
{
    public class KnightController : ServantController, IWarrior
    {
        public Action OnAttack { get; set; }
        public EntityController Controller => this;
        public float AttackRadius =>  _attackRadius + Radius;
        public string AttackSound => _attackSound;
        public int AttackDamage { get; set; }
        public float AttackSpeed { get; set; }

        [SerializeField] private float _attackRadius;
        [SerializeField] private SpriteRenderer _body;
        [SerializeField] private SpriteRenderer _sword;
        [SerializeField] private string _attackSound;

        public override void StartFirstState()
        {
        }
        
        public void Attack()
        {
            OnAttack?.Invoke();
        }
        
        protected new void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Radius + AttackRadius);
        }

        public void SetBodySprite(Sprite body)
        {
            _body.sprite = body;
        }

        public void SetSwordSprite(Sprite sword)
        {
            _sword.sprite = sword;
        }

        public void SetFootSprite(Sprite foot)
        {
            var footController = GetComponentInChildren<FootController>();
            footController.SetFootSprites(foot);
        }
    }
}