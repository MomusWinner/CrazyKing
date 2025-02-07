using System;
using Controllers.SoundManager;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace UI.Upgrade.PositioningTab
{
    public class TrashZone : MonoBehaviour, IDropHandler
    {
        public Action<ServantCard> OnThrowToTrash;
        [Inject] private SoundManager _soundManager;
        public void OnDrop(PointerEventData eventData)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(1.1f, 0.2f));
            sequence.Append(transform.DOScale(1f, 0.2f));
            _soundManager.StartMusic("Delete", SoundChannel.UI);
            GameObject dropped = eventData.pointerDrag;
            ServantCard card = dropped.GetComponent<ServantCard>();
            OnThrowToTrash?.Invoke(card);
        }
    }
}