using Entity.Servant;
using Servant;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace UI.Upgrade.ServantShop
{
    public class ServantShopItemContainer : MonoBehaviour
    {
        [SerializeField] private GameObject shopItemPrefab;
        [Inject] private ServantsSO _servantsSO;
        [Inject] private IObjectResolver _container;

        public void Start()
        {
            foreach (var servant in _servantsSO.AvailableServants)
            {
                var shopItem = _container.Instantiate(shopItemPrefab, transform).GetComponent<ServantShopItem>();
                shopItem.SetUp(servant);
            }
        }
    }
}