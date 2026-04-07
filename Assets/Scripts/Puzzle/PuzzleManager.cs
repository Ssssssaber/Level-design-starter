using System.Collections.Generic;
using UnityEngine;

namespace PuzzleSystem
{
    public class PuzzleManager : MonoBehaviour
    {
        [System.Serializable]
        public class PuzzlePart
        {
            public GameObject puzzleObject;
            public PuzzleState requiredState;

            public IPuzzleElement GetPuzzleElement()
            {
                if (puzzleObject == null) return null;

                IPuzzleElement[] found = puzzleObject.GetComponents<IPuzzleElement>();
                if (found.Length > 1)
                {
                    Debug.LogError($"PuzzleManager: PuzzlePart '{puzzleObject.name}' has multiple IPuzzleElement components! Only one is allowed.", puzzleObject);
                    return null;
                }
                if (found.Length == 1) return found[0];

                found = puzzleObject.GetComponentsInChildren<IPuzzleElement>();
                if (found.Length > 1)
                {
                    Debug.LogError($"PuzzleManager: PuzzlePart '{puzzleObject.name}' has multiple IPuzzleElement components in children! Only one is allowed.", puzzleObject);
                    return null;
                }
                if (found.Length == 1) return found[0];

                return null;
            }
        }

        [System.Serializable]
        public class PuzzleTarget
        {
            public GameObject targetObject;
            public PuzzleState targetState;

            public IPuzzleElement GetPuzzleElement()
            {
                if (targetObject == null) return null;

                IPuzzleElement[] found = targetObject.GetComponents<IPuzzleElement>();
                if (found.Length > 1)
                {
                    Debug.LogError($"PuzzleManager: PuzzleTarget '{targetObject.name}' has multiple IPuzzleElement components! Only one is allowed.", targetObject);
                    return null;
                }
                if (found.Length == 1) return found[0];

                found = targetObject.GetComponentsInChildren<IPuzzleElement>();
                if (found.Length > 1)
                {
                    Debug.LogError($"PuzzleManager: PuzzleTarget '{targetObject.name}' has multiple IPuzzleElement components in children! Only one is allowed.", targetObject);
                    return null;
                }
                if (found.Length == 1) return found[0];

                return null;
            }
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
        [Tooltip("Reset targets to initial state when puzzle becomes unsolved")]
        public bool resetOnUnsolved = true;

        private bool _isSolved = false;
        private Dictionary<int, PuzzleState> _targetInitialStates = new Dictionary<int, PuzzleState>();

        private void OnEnable()
        {
            PuzzleEvents.OnElementStateChanged += OnElementChanged;
        }

        private void OnDisable()
        {
            PuzzleEvents.OnElementStateChanged -= OnElementChanged;
        }

        private void OnValidate()
        {
            ValidatePuzzleParts();
            ValidatePuzzleTargets();
        }

        private void ValidatePuzzleParts()
        {
            foreach (var part in puzzleParts)
            {
                if (part.puzzleObject == null) continue;

                int selfCount = part.puzzleObject.GetComponents<IPuzzleElement>().Length;
                int childrenCount = part.puzzleObject.GetComponentsInChildren<IPuzzleElement>().Length;

                if (selfCount + childrenCount == 0)
                {
                    Debug.LogError($"PuzzleManager: '{gameObject.name}' - PuzzlePart '{part.puzzleObject.name}' has no IPuzzleElement component!", part.puzzleObject);
                }
                else if (selfCount + childrenCount > 1)
                {
                    Debug.LogError($"PuzzleManager: '{gameObject.name}' - PuzzlePart '{part.puzzleObject.name}' has multiple ({selfCount + childrenCount}) IPuzzleElement components! Only one is allowed.", part.puzzleObject);
                }
            }
        }

        private void ValidatePuzzleTargets()
        {
            foreach (var target in puzzleTargets)
            {
                if (target.targetObject == null) continue;

                int childrenCount = target.targetObject.GetComponentsInChildren<IPuzzleElement>().Length;

                if (childrenCount == 0)
                {
                    Debug.LogError($"PuzzleManager: '{gameObject.name}' - PuzzleTarget '{target.targetObject.name}' has no IPuzzleElement component!", target.targetObject);
                }
                else if (childrenCount > 1)
                {
                    Debug.LogError($"PuzzleManager: '{gameObject.name}' - PuzzleTarget '{target.targetObject.name}' has multiple ({childrenCount}) IPuzzleElement components! Only one is allowed.", target.targetObject);
                }
            }
        }

        private void OnElementChanged(IPuzzleElement element)
        {
            if (!checkOnPartChanged) return;

            if (IsPartInArray(element))
            {
                CheckPuzzleStatus();
            }
        }

        private void Update()
        {
            if (!continuousCheck) return;
            CheckPuzzleStatus();
        }

        public void CheckPuzzleStatus()
        {
            bool wasSolved = _isSolved;
            bool isNowSolved = IsPuzzleSolved();

            if (isNowSolved && !wasSolved)
            {
                _isSolved = true;
                StoreTargetInitialStates();
                ActivateTargets();
            }
            else if (!isNowSolved && wasSolved && resetOnUnsolved)
            {
                _isSolved = false;
                ResetTargetsToInitialState();
            }
        }

        private void StoreTargetInitialStates()
        {
            _targetInitialStates.Clear();
            for (int i = 0; i < puzzleTargets.Count; i++)
            {
                var element = puzzleTargets[i].GetPuzzleElement();
                if (element != null)
                {
                    _targetInitialStates[i] = element.InitialState;
                }
            }
        }

        private void ResetTargetsToInitialState()
        {
            for (int i = 0; i < puzzleTargets.Count; i++)
            {
                var element = puzzleTargets[i].GetPuzzleElement();
                if (element != null && _targetInitialStates.TryGetValue(i, out var initialState))
                {
                    element.SetState(initialState);
                }
            }
        }

        private bool IsPartInArray(IPuzzleElement element)
        {
            foreach (var part in puzzleParts)
            {
                var e = part.GetPuzzleElement();
                if (e != null && e == element)
                    return true;
            }
            return false;
        }

        public bool IsPuzzleSolved()
        {
            foreach (var part in puzzleParts)
            {
                var element = part.GetPuzzleElement();
                if (element != null)
                {
                    if (element.CurrentState != part.requiredState)
                    {
                        return false;
                    }
                }
                else
                {
                    Debug.LogWarning($"Puzzle part {part.puzzleObject?.name} does not have IPuzzleElement", part.puzzleObject);
                    return false;
                }
            }
            return puzzleParts.Count > 0;
        }

        private void ActivateTargets()
        {
            foreach (var target in puzzleTargets)
            {
                var element = target.GetPuzzleElement();
                if (element != null)
                {
                    element.SetState(target.targetState);
                }
                else
                {
                    Debug.LogWarning($"Puzzle target {target.targetObject?.name} does not have IPuzzleElement", target.targetObject);
                }
            }
        }

        public void ResetPuzzle()
        {
            _isSolved = false;
            if (resetOnUnsolved)
            {
                ResetTargetsToInitialState();
            }
        }
    }
}
