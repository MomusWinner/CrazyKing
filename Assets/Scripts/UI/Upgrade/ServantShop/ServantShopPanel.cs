using UnityEngine;

namespace UI.Upgrade.ServantShop
{
    public class ServantShopPanel : MonoBehaviour
    {
        [SerializeField] private GameObject shopPanel;

        public void OpenPanel()
        {
            shopPanel.SetActive(true);
        }

        public void ClosePanel()
        {
            shopPanel.SetActive(false);
        }
    }
}