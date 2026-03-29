using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GameObjectsSound
{
    [Serializable]
    public class ObjectSoundEntry
    {
        public SoundID Sound;
        public AudioClip Clip;
        [Range(0f, 1f)] public float Volume = 1.0f;
    }

    [CreateAssetMenu(fileName = "SoundProfile", menuName = "GameAudio/Sound Profile")]
    public class SoundProfile : ScriptableObject
    {
        public List<ObjectSoundEntry> Sounds = new();

        public ObjectSoundEntry GetSoundEntry(SoundID sound)
        {
            return Sounds.FirstOrDefault(s => s.Sound == sound);
        }
    }
}