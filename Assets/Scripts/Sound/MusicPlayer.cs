using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _soundFXObject;
    [SerializeField] private GlobalSoundEntry _defaultGlobalSound;

    private AudioSource _currentAudioSource;

    public void PlaySound(GlobalSoundEntry globalSoundEntry, Transform spawnTransform)
    {
        if (_currentAudioSource != null)
        {
            Destroy(_currentAudioSource.gameObject);
        }

        _currentAudioSource = Instantiate(_soundFXObject, spawnTransform.position, Quaternion.identity);
        _currentAudioSource.clip = globalSoundEntry.Clip;
        _currentAudioSource.volume = globalSoundEntry.Volume;
        _currentAudioSource.Play();
    }

    public void PlayDefaultGlobalSound()
    {
        if (_defaultGlobalSound == null) return;

        PlaySound(_defaultGlobalSound, transform);
    }
}