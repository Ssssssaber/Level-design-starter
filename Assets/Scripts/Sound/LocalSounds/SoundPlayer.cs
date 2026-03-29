using UnityEngine;


namespace GameObjectsSound
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _soundFXObject;
        [SerializeField] private GlobalSoundEntry _defaultGlobalSound;

        public void PlaySound(SoundID targetSound, SoundProfile targetProfile, Transform spawnTransform)
        {
            var soundEntry = targetProfile.GetSoundEntry(targetSound);
            if (soundEntry == null)
            {
                Debug.LogWarning($"Sound {targetSound} wasnt found in profile {targetProfile.name}");
                return;
            }

            AudioSource audioSource = Instantiate(_soundFXObject, spawnTransform.position, Quaternion.identity);
            audioSource.clip = soundEntry.Clip;
            audioSource.volume = soundEntry.Volume;
            audioSource.Play();

            Destroy(audioSource.gameObject, soundEntry.Clip.length);
        }

        public void PlaySound(GlobalSoundEntry globalSoundEntry, Transform spawnTransform)
        {
            AudioSource audioSource = Instantiate(_soundFXObject, spawnTransform.position, Quaternion.identity);
            audioSource.clip = globalSoundEntry.Clip;
            audioSource.volume = globalSoundEntry.Volume;
            audioSource.Play();

            Destroy(audioSource.gameObject, globalSoundEntry.Clip.length);
        }

        public void PlayDefaultGlobalSound()
        {
            if (_defaultGlobalSound == null) return;

            PlaySound(_defaultGlobalSound, transform);
        }
    }
}