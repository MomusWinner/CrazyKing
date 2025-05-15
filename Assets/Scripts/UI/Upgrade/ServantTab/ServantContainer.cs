using System.Collections.Generic;
using System.Linq;
using Entity.Servant;
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
        [Inject] private ServantStorage _servantStorage;
        [Inject] private IObjectResolver _container;
        private List<ServantItem> _servantItems = new();
        
        public void Start()
        {
            SetupServants();
            _servantStorage.OnAddServant += AddNewServant;
            _servantStorage.OnRemoveServant += RemoveServant;
        }
        
        // [Inject] private EvolutionPanel _evolutionPanel;
        // [Inject] private ServantsSO _servantsSO;
        // private void Evolution()
        // {
        //     _evolutionPanel.StartAnim(_servantsSO.GetServantByType(ServantType.Archer), new int[] {5, 5, 5});
        // }

        public void OnDestroy()
        {
            _servantStorage.OnRemoveServant -= RemoveServant;
            _servantStorage.OnAddServant -= AddNewServant;
        }

        private void SetupServants()
        {
             _servantItems.ForEach(item => Destroy(item.gameObject));
             _servantItems.Clear();
             
             foreach (var servant in _servantStorage.Servants)
             {
                 AddNewServant(servant);
             }        
        }

        private void RemoveServant(int id)
        {
            foreach (var item in _servantItems.Where(item => item.ServantData.ID == id).ToList())
            {
                if (item != null)
                    Destroy(item.gameObject);
                item.OnPointerEnterEvent -= OnServantItemPointerEnter;
                item.OnPointerExitEvent -= OnServantItemPointerExit;
                _servantItems.Remove(item);
            }
        }

        private void AddNewServant(ServantData servantData)
        {
            var servantItem = _container.Instantiate(_servantItemPref, _servantItemParent.transform).GetComponent<ServantItem>();
            servantItem.Setup(servantData);
            servantItem.OnPointerEnterEvent += OnServantItemPointerEnter;
            servantItem.OnPointerExitEvent += OnServantItemPointerExit;
            _servantItems.Add(servantItem);               
        }

        private void OnServantItemPointerEnter(ServantItem servantItem)
        {
            servantItem.OpenParameters();
        }

        private void OnServantItemPointerExit(ServantItem servantItem)
        {
            servantItem.CloseParameters();
        }
    }
}