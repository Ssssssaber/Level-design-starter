using Cinemachine;
using Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set; }

    [SerializeField] private PlayerStateMachine _playerPrefab;
    [SerializeField] private CinemachineVirtualCamera _cameraPrefab;
    [SerializeField] private Transform _spawnTransform;

    [Header("Debug")]
    public PlayerStateMachine Player;
    public CinemachineVirtualCamera Camera;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("game mangaer already exists");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InitPlayer(Instantiate(_playerPrefab, _spawnTransform.position, Quaternion.identity));
        Camera = Instantiate(_cameraPrefab, _spawnTransform.position, Quaternion.identity);
        Camera.m_Follow = Player.transform;
        _spawnTransform.gameObject.SetActive(false);
    }

    public void InitPlayer(PlayerStateMachine player)
    {
       if (Player != null)
       {
            Debug.LogError("player exits");
            return;
       }

       Player = player;
    }
}