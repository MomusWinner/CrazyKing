using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Upgrade.PositioningTab
{
    public class PositionSlot : MonoBehaviour, IDropHandler
    {
        public bool IsEmpty { get; private set; } = true;
        public ServantCard ServantCard => _currentCard;
        public int PositionId => id;
        public Action<ServantCard> OnDetectDrop;
        [SerializeField] private int id;
        private ServantCard _currentCard;
        
        public void SetServantCard(ServantCard card)
        {
            IsEmpty = false;
            _currentCard = card;
            card.transform.SetParent(transform);
        }

        public void OnDrop(PointerEventData eventData)
        {
            GameObject dropped = eventData.pointerDrag;
            ServantCard card = dropped.GetComponent<ServantCard>(); 
            OnDetectDrop?.Invoke(card);
        }

        public void DropCard()
        {
            IsEmpty = true;
            _currentCard = null;
        }
    }
}