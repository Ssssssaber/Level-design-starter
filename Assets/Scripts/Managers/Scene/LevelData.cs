using UnityEngine;
using System.Collections.Generic;
using Unity.VectorGraphics;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public string levelDisplayName;

    [Header("Additional Scenes to Load")]
    [SerializeField] private List<string> _levelScenes = new();

    public IReadOnlyList<string> LevelScenes => _levelScenes;
}
