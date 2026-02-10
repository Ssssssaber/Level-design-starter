using UnityEditor.Callbacks;
using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 20f;
    [SerializeField] private float _maxVelocity = 7f;
    [SerializeField] private float _linearDamping = 4f;

    private Vector2 _direction;
    private Rigidbody2D _body;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _body.linearDamping = _linearDamping;
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    } 

    private void ApplyMovement()
    {
        if (_direction == Vector2.zero) return;

        _body.AddForce(_direction * _moveSpeed);

        if (_body.linearVelocity.magnitude > _maxVelocity)
        {
            _body.linearVelocity = _body.linearVelocity.normalized * _maxVelocity;
        }
    }
}
