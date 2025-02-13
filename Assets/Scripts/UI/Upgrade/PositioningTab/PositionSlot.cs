using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Upgrade.PositioningTab
{
    public class PositionSlot : MonoBehaviour, IDropHandler
    {
        public bool Blocked { get; private set; } = false;
        public bool IsEmpty { get; private set; } = true;
        public ServantCard ServantCard => _currentCard;
        public int PositionId => id;
        public Action<ServantCard> OnDetectDrop;
        [SerializeField] private int id;
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _activeSprite;
        [SerializeField] private Sprite _blockSprite;
        private ServantCard _currentCard;

        public void SetServantCard(ServantCard card)
        {
            IsEmpty = false;
            _currentCard = card;
            card.transform.SetParent(transform);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (Blocked) return;
            GameObject dropped = eventData.pointerDrag;
            ServantCard card = dropped.GetComponent<ServantCard>(); 
            OnDetectDrop?.Invoke(card);
        }

        public void DropCard()
        {
            IsEmpty = true;
            _currentCard = null;
        }

        public void BlocK()
        {
            if (Blocked) return;
            _image.sprite = _blockSprite;
            Blocked = true;
        }

        public void Unblock()
        {
            if (!Blocked) return;
            _image.sprite = _activeSprite;
            Blocked = false;
        }
    }
}