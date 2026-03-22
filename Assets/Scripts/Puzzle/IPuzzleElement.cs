using UnityEngine;

namespace PuzzleSystem
{
    public interface IPuzzleElement
    {
        PuzzleState CurrentState { get; }
        void SetState(PuzzleState state);
    }
}
