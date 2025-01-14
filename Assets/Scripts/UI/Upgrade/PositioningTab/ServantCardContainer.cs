using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Upgrade.PositioningTab
{
    public class ServantCardContainer : MonoBehaviour, IDropHandler, IDisposable
    {
        private List<ServantCard> _servantCards = new ();
        public Action<ServantCard> OnDetectDrop;
        
        public void OnDrop(PointerEventData eventData)
        {
            GameObject dropped = eventData.pointerDrag;
            ServantCard card = dropped.GetComponent<ServantCard>();
            OnDetectDrop?.Invoke(card);
        }

        public void AddServantCard(ServantCard servantCard)
        {
            servantCard.transform.SetParent(transform, false);
            _servantCards.Add(servantCard); 
        }

        public ServantCard FindServantCardById(int id)
        {
            return _servantCards.Find(c => c.ServantData.ID == id);
        }

        public void Dispose()
        {
            _servantCards.Clear();
        }

        public void DropCard(int id)
        {
            _servantCards = _servantCards.Where(c => c.ServantData.ID != id).ToList();
        }
    }
}