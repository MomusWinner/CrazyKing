using System;
using Servant;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using UnityEngine.EventSystems;

namespace UI.Upgrade.PositioningTab
{
    public class ServantCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public ServantData ServantData {get; private set;}
        public Action OnDropped;
        public Action OnStartDrag;
        [SerializeField] private Image avatar;
        [SerializeField] private TMP_Text lv;
        [Inject] private ServantsSO _servantsSO;

        public void Setup(ServantData servantData)
        {
            ServantData = servantData;
            ServantSO servantSo = _servantsSO.GetServantByType(servantData.Type);
            avatar.sprite = servantSo.avatar;
            lv.text = servantData.Lv.ToString();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            avatar.raycastTarget = false;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            OnStartDrag?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            avatar.raycastTarget = true;
            OnDropped?.Invoke();
        }
    }
}