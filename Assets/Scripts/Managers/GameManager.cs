using Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set; }
    public PlayerStateMachine Player;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("game mangaer already exists");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}