using UnityEngine;
using Health;
using UnityEditor.Experimental.GraphView;

namespace RangedAttack
{
    public class Projectile : MonoBehaviour
    {
        [Header("Projectile parameters")]
        [SerializeField] private float _speed = 10f;
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _lifetime = 2f;

        [Header("References")]
        [SerializeField] private SpriteRenderer _sprite;

        private Vector2 _direction;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Launch(Vector2 direction)
        {
            _direction = direction.normalized;
            UpdateVisuals(direction);
            _rb.linearVelocity = _direction * _speed;
            Destroy(gameObject, _lifetime);
        }

        private void UpdateVisuals(Vector3 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            _sprite.flipX = direction.x > 0;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damage);
            }
            Destroy(gameObject);
        }
    }
}