using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Servant
{
    public class ServantFactory
    {
        private readonly IObjectResolver _container;
        
        [Inject]
        private ServantFactory(IObjectResolver container)
        {
            _container = container;
        }
        
        public void CreateKnight(Vector2 position)
        {
            var knightObj = Resources.Load<GameObject>("Servants/Knight");
            _container.Instantiate(knightObj, position, Quaternion.identity);
        }
    }
}