using Health;
using UnityEngine;

namespace Interactable
{
    public class HealthPotion : MonoBehaviour, IInteractable
    {
        public int HealAmount = 1;
        public bool CanInteract(GameObject interactor)
        {
            if (interactor.TryGetComponent(out HealthComponent health) && health.CurrentHealth < health.MaxHealth)
            {
                return true;
            }
            return false;
        }

        public void Interact(GameObject interactor)
        {
            if (interactor.TryGetComponent(out HealthComponent health) && health.CurrentHealth < health.MaxHealth)
            {
                if (health.Heal(HealAmount))
                {
                    Destroy(transform.parent.gameObject);
                }
            }
            else
            {
                Debug.LogError($"{interactor.name} has no health manger");
            }
        }
        public string GetTag()
        {
            return "HealthPotion";
        }

        public void OnInteractSound(GameObject interactor)
        {
            var localProfile = GetComponent<GameObjectsSound.SoundProfileContainer>()?.GetProfile();
            var profile = localProfile ?? interactor?.GetComponent<GameObjectsSound.SoundProfileContainer>()?.GetProfile();
            if (profile != null)
            {
                GameManager.Instance.FXSoundPlayer.PlaySound(GameObjectsSound.SoundID.Interact, profile, transform);
            }
        }
    }
}
