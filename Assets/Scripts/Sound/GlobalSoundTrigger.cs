using UnityEngine;
using GameObjectsSound;

public enum SoundType
{
    Music,
    FX
}

[RequireComponent(typeof(Collider2D))]
public class GlobalSoundTrigger : MonoBehaviour
{
    [SerializeField] private GlobalSoundEntry _globalSoundEntry;
    [SerializeField] private SoundType _soundType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.LogError("player entered");
        switch (_soundType)
        {
            case SoundType.Music:
                GameManager.Instance.MusicSoundPlayer.PlaySound(_globalSoundEntry, transform);
                break;

            case SoundType.FX:
                GameManager.Instance.FXSoundPlayer.PlaySound(_globalSoundEntry, transform);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.LogError("exited");
        switch (_soundType)
        {
            case SoundType.Music:
                GameManager.Instance.MusicSoundPlayer.PlayDefaultGlobalSound();
                break;

            case SoundType.FX:
                GameManager.Instance.FXSoundPlayer.PlayDefaultGlobalSound();
                break;
        }
    }
}