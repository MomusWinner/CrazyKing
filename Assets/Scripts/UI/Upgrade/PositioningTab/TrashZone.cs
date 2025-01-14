using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Upgrade.PositioningTab
{
    public class TrashZone : MonoBehaviour, IDropHandler
    {
        public Action<ServantCard> OnThrowToTrash;
        
        public void OnDrop(PointerEventData eventData)
        {
            GameObject dropped = eventData.pointerDrag;
            ServantCard card = dropped.GetComponent<ServantCard>();
            OnThrowToTrash?.Invoke(card);
        }
    }
}