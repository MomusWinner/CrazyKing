using BaseEntity;
using Health;
using Servant;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace King
{
    [RequireComponent(typeof(CirclePointController))]
    public class KingController : BaseEntityController
    {
        private ServantFactory _servantFactory;
        private CirclePointController _pointController;
        
        [Inject]
        public void Setup(ServantFactory servantFactory)
        {
            _servantFactory = servantFactory;
            _pointController = GetComponent<CirclePointController>();
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