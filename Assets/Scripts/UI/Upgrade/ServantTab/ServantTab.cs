using System.Collections.Generic;
using King;
using Servant;
using UnityEngine;
using VContainer;

namespace UI.Upgrade.ServantTab
{
    public class ServantTab : MonoBehaviour
    { 
        [SerializeField] private GameObject _servantItemPref;
        [SerializeField] private GameObject _servantItemParent;
        
        [Inject] private ServantManager _servantManager;
        [Inject] private KingController _king;

        private List<ServantItem> _servantItems = new();
        
        public void Start()
        {
             OnKingServantsChanged(_servantManager.KingServants);
             _servantManager.onKingServantsChanged += OnKingServantsChanged;
        }
        
        public void OnKingServantsChanged(IEnumerable<ServantController> servants)
        {
            _servantItems.ForEach(item => Destroy(item));
            
            foreach (var servant in servants)
            {
                var servantItem = Instantiate(_servantItemPref, _servantItemParent.transform).GetComponent<ServantItem>();
                servantItem.Setup(servant);
                _servantItems.Add(servantItem);    
            }
        }
    }
}