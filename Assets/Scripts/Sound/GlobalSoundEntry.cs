
using UnityEngine;

[CreateAssetMenu(fileName = "SoundProfile", menuName = "GameAudio/Global Sound")]
public class GlobalSoundEntry : ScriptableObject
{
    public AudioClip Clip;
    [Range(0f, 1f)] public float Volume = 1.0f;
}
