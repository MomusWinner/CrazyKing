using Controllers.Coins;
using Controllers.SoundManager;
using Entity.Servant;
using Servant;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.Upgrade.ServantShop
{
    public class ServantShopItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _servantName;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private Image _avatar;
        [SerializeField] private BuyButton _buyButton;
        
        [Inject] private CoinsManager _coinsManager;
        [Inject] private ServantStorage _servantStorage;
        [Inject] private SoundManager _soundManager;

        private bool _isDisable;
        private ServantSO _servant;
        
        public void SetUp(ServantSO servant)
        {
            _servant = servant;
            _servantName.text = servant.servantName;
            _description.text = servant.description;
            _buyButton.SetText(CoinsManager.Short(servant.price));
            _avatar.sprite = servant.GetAvatarByLevel(0);
            
            CheckBuyState(_coinsManager.CurrentCoins, 0);
            _coinsManager.OnIncrease += CheckBuyState;
            _coinsManager.OnDecrease += CheckBuyState;
        }
        
        public void OnDestroy()
        {
            _coinsManager.OnIncrease -= CheckBuyState;
            _coinsManager.OnDecrease -= CheckBuyState;
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
            _buyButton.SetState(BuyButtonState.Disable);
        }

        public void EnableBuyButton()
        {
            if (!_isDisable) return;
            _isDisable = false;
            _buyButton.SetState(BuyButtonState.Enable);
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