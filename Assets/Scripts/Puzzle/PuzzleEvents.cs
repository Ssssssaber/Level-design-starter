using UnityEngine;
using System;

namespace PuzzleSystem
{
    public static class PuzzleEvents
    {
        public static event Action<IPuzzleElement> OnElementStateChanged;

        public static void NotifyStateChanged(IPuzzleElement element)
        {
            OnElementStateChanged?.Invoke(element);
        }
    }
}
