using Controllers;
using Controllers.Coins;
using Controllers.SoundManager;
using Servant;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.Upgrade.ServantShop
{
    public class ServantShopItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text servantName;
        [SerializeField] private TMP_Text description;
        [SerializeField] private TMP_Text price;
        [SerializeField] private Image avatar;
        
        [Inject] private CoinsManager _coinsManager;
        [Inject] private ServantStorage _servantStorage;
        [Inject] private SoundManager _soundManager;

        private ServantSO _servant;
        
        public void SetUp(ServantSO servant)
        {
            _servant = servant;
            servantName.text = servant.servantName;
            description.text = servant.description;
            price.text = servant.price.ToString();
            avatar.sprite = servant.GetAvatarByLevel(0);
        }

        public void Buy()
        {
            if (!_coinsManager.TryGetCoins(_servant.price))
            {
                Debug.LogWarning("Not enough coins to buy a servant.");
                return;
            }
            _soundManager.StartMusic("Buy", SoundChannel.UI);
            _servantStorage.AddServant(new ServantData()
            {
                IsUsed = false,
                Lv = 0,
                Type = _servant.type
            });
        }
    }
}