using System;
using BaseEntity;
using Health;
using King.Upgrades;
using King.Upgrades.Parameters;
using Servant;
using UnityEngine;
using VContainer;

namespace King
{
    [RequireComponent(typeof(CirclePointController))]
    public class KingController : BaseEntityController
    {
        public KingParameterUpgradeController[] KingParameterUpgradeControllers => _kingParameterUpgradeControllers;
        
        private KingParameterUpgradeController[] _kingParameterUpgradeControllers;
        private ServantFactory _servantFactory;
        private CirclePointController _pointController;
        
        [Inject]
        public void Setup(ServantFactory servantFactory)
        {
            _servantFactory = servantFactory;
            _pointController = GetComponent<CirclePointController>();
            _kingParameterUpgradeControllers = new KingParameterUpgradeController[]
            {
                new KingHealthParameterUpgradeController(this, new KingUpgrade[]
                {
                    new KingHealthParameter1Upgrade(),
                    new KingHealthParameter1Upgrade(),
                    new KingHealthParameter1Upgrade(),
                    new KingHealthParameter1Upgrade(),
                })
            };
        }
        
        protected override void Start()
        {
            base.Start();
            _servantFactory.CreateKnight(transform.position);
            _servantFactory.CreateKnight(transform.position);
            _servantFactory.CreateKnight(transform.position);
            _servantFactory.CreateKnight(transform.position);
            _servantFactory.CreateKnight(transform.position);
        }
        
        public IPoint GetFreePoint() => _pointController.GetFreePoint();
        
        public void ReturnPoint(IPoint point) => _pointController.ReturnPoint(point);
    }
}