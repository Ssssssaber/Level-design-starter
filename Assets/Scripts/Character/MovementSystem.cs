using UnityEngine;

namespace Movement
{
    public class MovementSystem : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        private Vector2 _currentDirection;
        private Rigidbody2D _rigidbody;
        private bool _canMove = true; // Flag to enable/disable movement

        public Vector2 CurrentDirection => _currentDirection;
        public bool CanMove
        {
            get => _canMove;
            set => _canMove = value;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (_canMove)
            {
                _rigidbody.linearVelocity = _currentDirection * _moveSpeed;
                return;
            }
            _rigidbody.linearVelocity = Vector2.zero;
        }

        public void SetDirection(Vector2 direction)
        {
            _currentDirection = direction;
        }
    }
}
