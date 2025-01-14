using System.Collections.Generic;
using System.Linq;
using Servant;
using UnityEditor;
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
            SetupServants();
            _servantsStorage.OnAddServant += AddNewServant;
            _servantsStorage.OnRemoveServant += id =>
            {
                foreach (var item in _servantItems.Where(item => item.ServantData.ID == id))
                    Destroy(item.gameObject);
            };
        }

        private void SetupServants()
        {
             _servantItems.ForEach(item => Destroy(item.gameObject));
             _servantItems.Clear();
             
             foreach (var servant in _servantsStorage.Servants)
             {
                 AddNewServant(servant);
             }           
        }

        private void AddNewServant(ServantData servantData)
        {
            var servantItem = _container.Instantiate(_servantItemPref, _servantItemParent.transform).GetComponent<ServantItem>();
            servantItem.Setup(servantData);
            _servantItems.Add(servantItem);               
        }
    }
}