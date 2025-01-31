using Controllers.SoundManager;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class MusicUI: MonoBehaviour
    {
        [SerializeField] private Slider _volumeSlider;
        [Inject] private SoundManager _soundManager;

        public void Start()
        {
            _volumeSlider.value = _soundManager.MusicVolume;    
        }

        public void SetValue(float value)
        {
            _soundManager.SetMusicVolume(value);
        }
    }
}