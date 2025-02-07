using System;
using UnityEngine;

namespace Controllers.SoundManager
{
    [Serializable]
    public class Sound
    {
        public string Name;
        public AudioClip Clip;
        [Range(0f, 1f)] public float Volume;
        [Range(.1f, 3f)] public float PitchMax = 1f;
        [Range(.1f, 3f)] public float PitchMin = 1f;
        public bool Loop;
        public int Priority;
    }
}