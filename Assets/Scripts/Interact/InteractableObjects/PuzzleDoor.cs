using PuzzleSystem;
using UnityEngine;

namespace Interactable
{
    public class PuzzleDoor : Door, IPuzzleElement
    {
        public PuzzleState CurrentState => _toggled ? PuzzleState.On : PuzzleState.Off;
        public PuzzleState InitialState => PuzzleState.On;

        public void SetState(PuzzleState state)
        {
            _toggled = state == PuzzleState.On;
            UdpateSprite(_toggled);
            UpdateColliderObstacle(_toggled);
            Debug.Log($"PuzzleDoor set to {state}");
        }
    }
}
