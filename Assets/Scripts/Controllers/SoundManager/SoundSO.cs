using UnityEngine;
using UnityEngine.Audio;

namespace Controllers.SoundManager
{
    [CreateAssetMenu(menuName = "Game/SoundSO")]
    public class SoundSO : ScriptableObject
    {
        public AudioMixerGroup SFXGroup;
        public AudioMixerGroup MusicGroup;
        public AudioMixer Mixer;
        public Sound[] Sounds;
    }
}