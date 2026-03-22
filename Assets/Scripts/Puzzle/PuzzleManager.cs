using System.Collections.Generic;
using UnityEngine;

namespace PuzzleSystem
{
    public class PuzzleManager : MonoBehaviour
    {
        [System.Serializable]
        public class PuzzlePart
        {
            public MonoBehaviour puzzleObject;
            public PuzzleState requiredState;
        }

        [System.Serializable]
        public class PuzzleTarget
        {
            public MonoBehaviour targetObject;
            public PuzzleState targetState;
        }

        [Header("Puzzle Parts")]
        [Tooltip("Objects that must be in a specific state for puzzle to complete")]
        public List<PuzzlePart> puzzleParts = new();

        [Header("Puzzle Targets")]
        [Tooltip("Objects that will be set to a specific state when puzzle completes")]
        public List<PuzzleTarget> puzzleTargets = new();

        [Header("Settings")]
        public bool checkOnPartChanged = true;
        [Tooltip("Check puzzle status in Update (for elements that change without events)")]
        public bool continuousCheck = false;

        private bool _isSolved = false;

        private void OnEnable()
        {
            PuzzleEvents.OnElementStateChanged += OnElementChanged;
        }

        private void OnDisable()
        {
            PuzzleEvents.OnElementStateChanged -= OnElementChanged;
        }

        private void OnElementChanged(IPuzzleElement element)
        {
            if (!checkOnPartChanged || _isSolved) return;

            if (IsPartInArray(element))
            {
                CheckPuzzleStatus();
            }
        }

        private void Update()
        {
            if (!continuousCheck || _isSolved) return;
            CheckPuzzleStatus();
        }

        public void CheckPuzzleStatus()
        {
            if (_isSolved) return;

            if (IsPuzzleSolved())
            {
                _isSolved = true;
                ActivateTargets();
            }
        }

        private bool IsPartInArray(IPuzzleElement element)
        {
            foreach (var part in puzzleParts)
            {
                if (part.puzzleObject is IPuzzleElement e && e == element)
                    return true;
            }
            return false;
        }

        public bool IsPuzzleSolved()
        {
            foreach (var part in puzzleParts)
            {
                if (part.puzzleObject is IPuzzleElement element)
                {
                    if (element.CurrentState != part.requiredState)
                    {
                        return false;
                    }
                }
                else
                {
                    Debug.LogWarning($"Puzzle part {part.puzzleObject?.name} does not implement IPuzzleElement", part.puzzleObject);
                    return false;
                }
            }
            return puzzleParts.Count > 0;
        }

        private void ActivateTargets()
        {
            foreach (var target in puzzleTargets)
            {
                if (target.targetObject is IPuzzleElement element)
                {
                    element.SetState(target.targetState);
                }
                else
                {
                    Debug.LogWarning($"Puzzle target {target.targetObject?.name} does not implement IPuzzleElement", target.targetObject);
                }
            }
        }

        public void ResetPuzzle()
        {
            _isSolved = false;
        }
    }
}
