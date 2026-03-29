using System;
using System.Collections.Generic;
using Cinemachine;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Action GameStarted;
    public Action GameFinished;
    public Action MenuLoaded;

    [Header("Scene References")]
    [SerializeField] private string _menuScene;
    [SerializeField] private LevelData _currentLevel;

    [Header("References (Set before play mode)")]
    [SerializeField] private PlayerStateMachine _playerPrefab;
    [SerializeField] private CinemachineVirtualCamera _cameraPrefab;
    [SerializeField] private Transform _spawnTransform;

    [Header("DEBUG (Set in play mode)")]
    public PlayerStateMachine Player;
    public CinemachineVirtualCamera Camera;

    private readonly HashSet<string> _levelScenes = new();
    private bool _isMenuLoaded;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("game manager already exists");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadMenu();
    }

    private void LoadMenu()
    {
        StartCoroutine(LoadMenuRoutine());
    }

    private System.Collections.IEnumerator LoadMenuRoutine()
    {
        if (!string.IsNullOrEmpty(_menuScene))
        {
            AsyncOperation loadOp = SceneManager.LoadSceneAsync(_menuScene, LoadSceneMode.Additive);
            yield return new WaitUntil(() => loadOp.isDone);
            _isMenuLoaded = true;
            MenuLoaded?.Invoke();
        }
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

    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    private System.Collections.IEnumerator StartGameRoutine()
    {
        InitPlayer(Instantiate(_playerPrefab, _spawnTransform.position, Quaternion.identity));
        Camera = Instantiate(_cameraPrefab, _spawnTransform.position, Quaternion.identity);
        Camera.m_Follow = Player.transform;
        _spawnTransform.gameObject.SetActive(false);
        GameStarted?.Invoke();

        if (_isMenuLoaded && !string.IsNullOrEmpty(_menuScene))
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(_menuScene);
            yield return new WaitUntil(() => unloadOp.isDone);
            _isMenuLoaded = false;
        }

        foreach (var scene in _currentLevel.LevelScenes)
        {
            if (!string.IsNullOrEmpty(scene))
            {
                AsyncOperation loadOp = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
                yield return new WaitUntil(() => loadOp.isDone);
                _levelScenes.Add(scene);
            }
        }
    }

    public void ReturnToMenu()
    {
        StartCoroutine(ReturnToMenuRoutine());
    }

    private System.Collections.IEnumerator ReturnToMenuRoutine()
    {
        foreach (var sceneName in _levelScenes)
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                yield return SceneManager.UnloadSceneAsync(sceneName);
            }
        }
        _levelScenes.Clear();

        if (Player != null)
        {
            Destroy(Player.gameObject);
            Player = null;
        }

        if (Camera != null)
        {
            Destroy(Camera.gameObject);
            Camera = null;
        }

        if (_spawnTransform != null)
        {
            _spawnTransform.gameObject.SetActive(true);
        }

        if (!string.IsNullOrEmpty(_menuScene))
        {
            AsyncOperation loadOp = SceneManager.LoadSceneAsync(_menuScene, LoadSceneMode.Additive);
            yield return new WaitUntil(() => loadOp.isDone);
            _isMenuLoaded = true;
        }

        GameFinished?.Invoke();
    }

    public void QuitApplication()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
