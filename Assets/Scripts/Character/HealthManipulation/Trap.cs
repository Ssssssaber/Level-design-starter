using PuzzleSystem;
using UnityEngine;

namespace Health
{
    [RequireComponent(typeof(Animator))]
    public class Trap : MonoBehaviour, IPuzzleElement
    {
        [Header("Settings")]
        [SerializeField] private bool _isWorking = true;
        [Header("References")]
        [SerializeField] private Collider2D _damageCollider;

        private Animator _animator;
        private float _baseAnimatorSpeed;
        private bool _initialState;

        public bool IsWorking => _isWorking;

        public PuzzleState CurrentState => _isWorking ? PuzzleState.On : PuzzleState.Off;
        public PuzzleState InitialState => _initialState ? PuzzleState.On : PuzzleState.Off;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _baseAnimatorSpeed = _animator.speed;
            _initialState = _isWorking;
        }

        public void SetState(PuzzleState state)
        {
            switch (state)
            {
                case PuzzleState.On:
                    _isWorking = true;
                    _animator.speed = _baseAnimatorSpeed;
                    break;
                
                case PuzzleState.Off:
                    _isWorking = false;
                    break;

                default:
                    Debug.LogWarning($"State {state} not implemented for {gameObject.name}");
                    break;
            }
        }

        public void OnAnimationStart()
        {
            if (!_isWorking)
            {
                _animator.speed = 0;
            }
        }
    }
}  