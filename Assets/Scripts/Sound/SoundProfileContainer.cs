using UnityEngine;

namespace Sound
{
    public class SoundProfileContainer : MonoBehaviour
    {
        [SerializeField] private SoundProfile _soundProfile;
        public SoundProfile GetProfile()
        {
            return _soundProfile;
        }
    }    
}
