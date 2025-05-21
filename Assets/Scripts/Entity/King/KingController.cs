using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Controllers;
using Controllers.Coins;
using Entity.King.FSM;
using Entity.King.Upgrades;
using Entity.King.Upgrades.Parameters;
using Entity.Servant;
using Servant;
using UnityEngine;
using VContainer;

namespace Entity.King
{
    [RequireComponent(typeof(CirclePointController))]
    public class  KingController : BaseEntityController, IThrowable
    {
        public KingFSM Fsm => _kingFsm;
        public IList<ServantController> Servants => _servants.ToList().AsReadOnly();
        public int AttackDamage { get; private set; }
        
        private KingParameterUpgradeController[] _kingParameterUpgradeControllers;
        private CirclePointController _pointController;
        private readonly ObservableCollection<ServantController> _servants = new ();
        private CoinsManager _coinsManager;
        private KingParameterManager _kingParameterManager;
        private InputManager _inputManager;
        private KingFSM _kingFsm;
        private KingParametersSO _kingParametersSO;
        
        [Inject]
        public void Setup(CoinsManager coinsManager,
            KingParameterManager kingParameterManager,
            InputManager inputManager,
            IObjectResolver container,
            KingParametersSO parametersSO)
        {
            _kingFsm = new KingFSM(container, this);
            _kingParameterManager = kingParameterManager;
            _coinsManager = coinsManager;
            _pointController = GetComponent<CirclePointController>();
            _inputManager = inputManager;
            _inputManager.OnAttack += StartAttack;
            _inputManager.OnKick += Kick;
            _kingParametersSO = parametersSO;
        }
        
        protected override void Start()
        {
            base.Start();
            Initialize();
            ChangeMaxHealth(_kingParameterManager.GetParameterValue<int>(KingParameterType.Health));
            AttackDamage = _kingParameterManager.GetParameterValue<int>(KingParameterType.Damage);
        }

        private void Kick()
        {
            if (_kingFsm.currentState is KickState) return;
            if (_kingFsm.currentState is KingAttackState)
            {
                _kingFsm.SendMessage("next_state", typeof(KickState));
                return;
            }
            _kingFsm.ChangeState<KickState>();
        }

        private void StartAttack()
        {
            if (_kingFsm.currentState is KingAttackState) return;
            if (_kingFsm.currentState is KickState)
            {
                _kingFsm.SendMessage("next_state", typeof(KingAttackState));
                return;
            }
            _kingFsm.ChangeState<KingAttackState>();
        }

        public void SetDamage()
        {
            _kingFsm.SendMessage("set_damage");
        }

        public void SubscribeToServantsChanged(Action<IList<ServantController>> onChanged)
        {
            void OnServantsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => onChanged.Invoke(Servants);
            _servants.CollectionChanged +=  OnServantsOnCollectionChanged;
        }

        public void AddServant(ServantController servant)
        {
            _servants.Add(servant);
        }

        public bool RemoveServant(ServantController servant)
        {
            return _servants.Remove(servant);
        }

        public bool TryGetFreePoint(out IPoint freePoint)
            => _pointController.TryGetFreePoint(out freePoint);
        
        public bool TryGetPoint(int pointId, out IPoint point)
            => _pointController.TryGetPoint(pointId, out point);
        
        public bool TryGetPositionById(int pointId, out Vector2 position)
            => _pointController.TryGetPointPosition(pointId, out position);
        
        public void ReturnPoint(IPoint point) => _pointController.ReturnPoint(point);


        public void OnDrawGizmos()
        {
            if (_kingParametersSO is null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.right * _kingParametersSO.AttackDistance, _kingParametersSO.AttackRadius);
        }

        public void Throw(Vector2 direction, float force)
        {
            RigidBody.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }
}