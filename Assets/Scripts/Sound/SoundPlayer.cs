using UnityEngine;


namespace Sound
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _soundFXObject;
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
    }
}