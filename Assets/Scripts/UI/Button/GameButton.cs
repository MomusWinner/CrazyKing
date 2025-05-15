using Controllers.SoundManager;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace UI
{
    public class GameButton : MonoBehaviour, IPointerClickHandler
    {
        [Inject] private SoundManager _soundManager;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _soundManager.StartMusic("Click", SoundChannel.UI);
        }
    }
}