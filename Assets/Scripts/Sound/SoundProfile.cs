using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Sound
{
    [Serializable]
    public class SoundEntry
    {
        public SoundID Sound;
        public AudioClip Clip;
        [Range(0f, 1f)] public float Volume;
    }

    [CreateAssetMenu(fileName = "SoundProfile", menuName = "GameAudio/Sound Profile")]
    public class SoundProfile : ScriptableObject
    {
        public List<SoundEntry> Sounds = new();

        public SoundEntry GetSoundEntry(SoundID sound)
        {
            return Sounds.FirstOrDefault(s => s.Sound == sound);
        }
    }
}