using System.Collections.Generic;
using System.Linq;
using Servant;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace UI.Upgrade.PositioningTab
{
    public class PositioningManager : MonoBehaviour
    {
        [SerializeField] private List<PositionSlot> slots;
        [SerializeField] private GameObject servantCardPref;
        [SerializeField] private ServantCardContainer servantCardContainer;
        [SerializeField] private TrashZone trashZone;
        [Inject] private ServantStorage _servantStorage;
        [Inject] private IObjectResolver _container;
        private List<ServantCard> _servantCards = new();
        
        private ServantCard _currentCard;
        
        public void Start()
        {
            SetupCards(_servantStorage.Servants);
            trashZone.OnThrowToTrash += OnThrowToTrash;
            _servantStorage.OnAddServant += AddServantCard;
            _servantStorage.OnRemoveServant += RemoveServantCard;
            _servantStorage.OnUpgradeServant += UpgradeServantCard;
        }

        public void OnDestroy()
        {
            trashZone.OnThrowToTrash -= OnThrowToTrash;
            _servantStorage.OnAddServant -= AddServantCard;
            _servantStorage.OnRemoveServant -= RemoveServantCard;
            _servantStorage.OnUpgradeServant -= UpgradeServantCard;
        }
        
        private void SetupCards(IEnumerable<ServantData> servants)
        {
            Cleanup();
            
            slots.ForEach(s => s.OnDetectDrop += card => OnSlotDetectDrop(s, card));
            servantCardContainer.OnDetectDrop += OnCardContainerDetectDrop;
            foreach (var servantData in servants)
                AddServantCard(servantData);
        }

        private void Cleanup()
        {
             _servantCards.ForEach(s => Destroy(s.gameObject)); 
             servantCardContainer.Dispose();
             _servantCards.Clear();           
        }

        private void RemoveServantCard(int id)
        {
            foreach (var servantCard in _servantCards.Where(c => c.ServantData.ID == id))
                Destroy(servantCard.gameObject);
        }

        private void UpgradeServantCard(ServantData data)
        {
            foreach (var servantCard in _servantCards.Where(c => c.ServantData.ID == data.ID))
                servantCard.Setup(data);
        }

        private void AddServantCard(ServantData servantData)
        {
            ServantCard card = _container.Instantiate(servantCardPref).GetComponent<ServantCard>();
            card.Setup(servantData);
            _servantCards.Add(card);
            card.OnStartDrag += () => OnSomeCardStartDragging(card);
            if (servantData.IsUsed)
            {
                var slot = GetSlotByPositionId(servantData.PointId); 
                slot.SetServantCard(card);
            }
            else
                servantCardContainer.AddServantCard(card);
        }

        private PositionSlot GetSlotByPositionId(int id)
        {
            return slots.Find(s => s.PositionId == id);
        }

        private void OnCurrentCardDropped()
        {
             Transform parent = GetCardParent(_currentCard.ServantData.ID);
             _currentCard.transform.SetParent(parent);
             CleanupCurrentCard();
        }

        private void OnSomeCardStartDragging(ServantCard servantCard)
        {
            _currentCard = servantCard;
            _currentCard.OnDropped += OnCurrentCardDropped;
        }

        private void OnSlotDetectDrop(PositionSlot slot, ServantCard servantCard)
        {
            if(_currentCard == null) return;
            CleanupCurrentCard();
            if (!slot.IsEmpty)
            {
                PositionSlot previousSlot = GetSlotByServantId(servantCard.ServantData.ID);
                if (previousSlot != null)
                    SetCardToSlot(previousSlot, slot.ServantCard);
                if (servantCardContainer.FindServantCardById(servantCard.ServantData.ID) != null)
                    AddCardToServantCardContainer(slot.ServantCard);
            }
            SetCardToSlot(slot, servantCard);   
        }

        private void SetCardToSlot(PositionSlot slot, ServantCard card)
        {
            DropCard(card.ServantData.ID);
            _servantStorage.SetServantPoint(card.ServantData.ID, slot.PositionId);
            slot.SetServantCard(card);
        }
        
        private void OnCardContainerDetectDrop(ServantCard servantCard)
        {
            if(_currentCard is null) return;
            CleanupCurrentCard();
            AddCardToServantCardContainer(servantCard);
        }    
        
        private void AddCardToServantCardContainer(ServantCard servantCard)
        {
             DropCard(servantCard.ServantData.ID);
             _servantStorage.UnsetServantPoint(servantCard.ServantData.ID);
             servantCardContainer.AddServantCard(servantCard);            
        }
         
        private Transform GetCardParent(int id)
        {
            int index = slots.FindIndex(s => !s.IsEmpty && s.ServantCard.ServantData.ID == id);
            if (index != -1) return slots[index].transform;
            return servantCardContainer.FindServantCardById(id) is not null ? servantCardContainer.transform : null;
        }

        private void CleanupCurrentCard()
        {
            if (_currentCard is null) return;
            _currentCard.OnDropped -= OnCurrentCardDropped;
            _currentCard = null;
        }

        private void DropCard(int servantId)
        {
            slots.ForEach(s =>
            {
                if (!s.IsEmpty && s.ServantCard.ServantData.ID == servantId)
                    s.DropCard();
            });

            servantCardContainer.DropCard(servantId);
        }

        private void OnThrowToTrash(ServantCard card)
        {
            CleanupCurrentCard();
            _servantStorage.RemoveServant(card.ServantData.ID);
            Destroy(card.gameObject);
        }

        private PositionSlot GetSlotByServantId(int id)
        {
            return slots.FirstOrDefault(slot => !slot.IsEmpty && slot.ServantCard.ServantData.ID == id);
        }
    }
}