using UnityEngine;

namespace Collectable
{
    public class CollecatbleDetector : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out ICollectable collectable) && collectable.CanCollect(gameObject))
            {
                collectable.Collect(gameObject);
            }
        }
    }
}