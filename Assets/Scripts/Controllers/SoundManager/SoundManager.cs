using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Controllers.SoundManager
{
    public enum SoundChannel
    {
        Background,
        UI,
        Effect,
    }

    public class AudioPlayer
    {
        public int Priority;
        public AudioSource AudioSource;
    }
    
    public class SoundManager : IStartable
    {
        public float MusicVolume => _saveManager.GameData.MusicVolume;
        
        [Inject] private SaveManager _saveManager;
        [Inject] private SoundSO _soundSO;
        private readonly Dictionary<SoundChannel, AudioPlayer[]> _soundByChannel = new();
        private GameObject soundManagerObject;
        
        public void Start()
        {
            SetMusicVolume(_saveManager.GameData.MusicVolume);
            soundManagerObject = new GameObject("SoundManager");
            Object.DontDestroyOnLoad(soundManagerObject);
            _soundByChannel.Add(SoundChannel.Background, new[] { CreateAudioPlayer(_soundSO.MusicGroup) });
            _soundByChannel.Add(SoundChannel.UI, new[] { CreateAudioPlayer(_soundSO.SFXGroup) });
            _soundByChannel.Add(SoundChannel.Effect, new[]
            {
                CreateAudioPlayer(_soundSO.SFXGroup),
                CreateAudioPlayer(_soundSO.SFXGroup),
                CreateAudioPlayer(_soundSO.SFXGroup),
                CreateAudioPlayer(_soundSO.SFXGroup),
            });
        }

        public void StartMusic(string name, SoundChannel channel)
        {
            Sound sound = GetSound(name);
            if (sound == null)
            {
                Debug.LogError($"Sound {name} not found");
                return;
            }
            
            AudioPlayer audioPlayer = GetFreeAudioSource(channel);
            if (audioPlayer == null)
            {
                audioPlayer = GetAudioPlayerWith_LE_Priority(channel, sound.Priority);
                if (audioPlayer == null) return;
            }
            
            audioPlayer.Priority = sound.Priority;
            UpdateAudioSource(audioPlayer.AudioSource, sound);
            if (channel == SoundChannel.Background)
                audioPlayer.AudioSource.Play();
            else
                audioPlayer.AudioSource.PlayOneShot(audioPlayer.AudioSource.clip);
        }

        public void SetMusicVolume(float value)
        {
            _soundSO.Mixer.SetFloat("master", Mathf.Log10(value) * 20);
            _saveManager.GameData.MusicVolume = value;
            _saveManager.Save();
        }
        
        private AudioPlayer CreateAudioPlayer(AudioMixerGroup mixerGroup, int priority = 0)
        {
            var audioSource = soundManagerObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = mixerGroup;
            return new AudioPlayer { Priority = priority, AudioSource = audioSource };
        }

        private AudioPlayer GetFreeAudioSource(SoundChannel channel)
        {
            AudioPlayer[] audioPlayers = _soundByChannel[channel];
            return audioPlayers.FirstOrDefault(a => !a.AudioSource.isPlaying);
        }

        private Sound GetSound(string name)
        {
            return _soundSO.Sounds.FirstOrDefault(s => s.Name == name);
        }

        private void UpdateAudioSource(AudioSource audioSource, Sound sound)
        {
            audioSource.volume = sound.Volume;
            audioSource.pitch = Random.Range(sound.PitchMin, sound.PitchMax);
            audioSource.clip = sound.Clip;
            audioSource.loop = sound.Loop;
        }

        private AudioPlayer GetAudioPlayerWith_LE_Priority(SoundChannel channel, int priority)
        {
            AudioPlayer[] audioPlayers = _soundByChannel[channel];
            return
                (from audioPlayer in audioPlayers
                where audioPlayer.Priority <= priority// && !audioPlayer.AudioSource.isPlaying
                orderby audioPlayer.Priority ascending, !audioPlayer.AudioSource.isPlaying descending
                select audioPlayer).FirstOrDefault();
        }
    }
}