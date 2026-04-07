using UnityEngine;

namespace PuzzleSystem
{
    public interface IPuzzleElement
    {
        PuzzleState CurrentState { get; }
        PuzzleState InitialState { get; }
        void SetState(PuzzleState state);
    }
}
