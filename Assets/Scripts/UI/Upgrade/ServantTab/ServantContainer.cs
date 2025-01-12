using System.Collections.Generic;
using Servant;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace UI.Upgrade.ServantTab
{
    public class ServantContainer : MonoBehaviour
    { 
        [SerializeField] private GameObject _servantItemPref;
        [SerializeField] private GameObject _servantItemParent;
        [Inject] private ServantsStorage _servantsStorage;
        [Inject] private IObjectResolver _container;
        private List<ServantItem> _servantItems = new();
        
        public void Start()
        {
             OnKingServantsChanged(_servantsStorage.Servants);
             _servantsStorage.OnServantsUpdated += OnKingServantsChanged;
        }
        
        public void OnKingServantsChanged(IEnumerable<ServantData> servants)
        {
            _servantItems.ForEach(item => Destroy(item.gameObject));
            _servantItems.Clear();
            
            foreach (var servant in servants)
            {
                var servantItem = _container.Instantiate(_servantItemPref, _servantItemParent.transform).GetComponent<ServantItem>();
                servantItem.Setup(servant);
                _servantItems.Add(servantItem);    
            }
        }
    }
}