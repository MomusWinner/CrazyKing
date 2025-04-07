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
        [SerializeField] private Button _buyButton;
        
        [SerializeField] private Sprite _enableButtonSprite;
        [SerializeField] private Sprite _disableButtonSprite;
        
        [Inject] private CoinsManager _coinsManager;
        [Inject] private ServantStorage _servantStorage;
        [Inject] private SoundManager _soundManager;

        private bool _isDisable;
        private ServantSO _servant;
        
        public void SetUp(ServantSO servant)
        {
            _servant = servant;
            servantName.text = servant.servantName;
            description.text = servant.description;
            price.text = CoinsManager.Short(servant.price);
            avatar.sprite = servant.GetAvatarByLevel(0);
            
            CheckBuyState(_coinsManager.CurrentCoins, 0);
            _coinsManager.OnIncrease += CheckBuyState;
            _coinsManager.OnDecrease += CheckBuyState;
        }

        private void CheckBuyState(int coins, int _)
        {
            if (_servant.price > coins)
                DisableBuyButton();
            else
                EnableBuyButton();
        }

        public void DisableBuyButton()
        {
            if (_isDisable) return;
            _isDisable = true;
            _buyButton.image.sprite = _disableButtonSprite;
        }

        public void EnableBuyButton()
        {
            if (!_isDisable) return;
            _isDisable = false;
            _buyButton.image.sprite = _enableButtonSprite;
        }

        public void Buy()
        {
            if (!_coinsManager.TryGetCoins(_servant.price))
            {
                _soundManager.StartMusic("Block", SoundChannel.UI);
                Debug.LogWarning("Not enough coins to buy a servant.");
                return;
            }
            _soundManager.StartMusic("Buy", SoundChannel.UI);
            _servantStorage.AddServant(new ServantData()
            {
                IsUsed = false,
                Lv = 1,
                Type = _servant.type
            });
        }
    }
}