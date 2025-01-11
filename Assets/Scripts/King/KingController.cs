using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using BaseEntity;
using Controllers;
using King.Upgrades;
using King.Upgrades.Parameters;
using Servant;
using UnityEngine;
using VContainer;

namespace King
{
    [RequireComponent(typeof(CirclePointController))]
    public class  KingController : BaseEntityController
    {
        public KingParameterUpgradeController[] KingParameterUpgradeControllers => _kingParameterUpgradeControllers;
        
        public IList<ServantController> Servants => _servants.ToList().AsReadOnly();
        
        private KingParameterUpgradeController[] _kingParameterUpgradeControllers;
        private CirclePointController _pointController;
        private readonly ObservableCollection<ServantController> _servants = new ();
        private CoinsManager _coinsManager;

        [Inject]
        public void Setup(CoinsManager coinsManager)
        {
            _coinsManager = coinsManager;
            _pointController = GetComponent<CirclePointController>();
        }
        
        protected override void Start()
        {
            base.Start();
            // _servantFactory.CreateKnight(transform.position);
            // _servantFactory.CreateKnight(transform.position);
            // _servantFactory.CreateKnight(transform.position);
            // _servantFactory.CreateKnight(transform.position);
            // _servantFactory.CreateKnight(transform.position);
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
        
        public bool TryGetFreePoint(out IPoint point) => _pointController.TryGetFreePoint(out point);
        
        public void ReturnPoint(IPoint point) => _pointController.ReturnPoint(point);
    }
}