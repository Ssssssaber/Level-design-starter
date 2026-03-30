using PuzzleSystem;
using UnityEngine;

namespace Health
{
    [RequireComponent(typeof(Animator))]
    public class Trap : MonoBehaviour, IPuzzleElement
    {
        [SerializeField] private bool _isWorking = true;

        public bool IsWorking
        {
            get
            {
                return _isWorking;
            }
            private set
            {
                _isWorking = value;
                
            }
        }

        private Animator _animator;

        public PuzzleState CurrentState => throw new System.NotImplementedException();

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            IsWorking = _isWorking;
        }

        public void SetState(PuzzleState state)
        {
            switch (state)
            {
                case PuzzleState.On:
                    IsWorking = true;
                    break;
                
                case PuzzleState.Off:
                    IsWorking = false;
                    break;

                default:
                    Debug.LogWarning($"State {state} not implemented for {gameObject.name}");
                    break;
            }
        }

        public void OnAnimationStart()
        {
            if (_isWorking)
            {
                _animator.StopPlayback();
            }
            else
            {
                _animator.StartPlayback();
            }
        }
    }
}
