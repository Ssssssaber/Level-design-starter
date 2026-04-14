using System;
using System.Collections.Generic;
using System.IO;
using Cinemachine;
using Player;
using GameObjectsSound;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vladimir.Utils;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public SoundPlayer FXSoundPlayer { get; private set; }
    public MusicPlayer MusicSoundPlayer { get; private set; }

    public bool IsGameStarted { get; private set; } 

    public Action GameStarted;
    public Action GameWon;
    public Action GameLost;
    public Action GameMenuRequested;
    public Action GameFinished;
    public Action MenuLoaded;

    [Header("Scene References")]
    [SerializeField] private string _menuScene;
    private string _environmentScene;
    [SerializeField] private LevelData _currentLevel;

    [Header("References (Set before play mode)")]
    [SerializeField] private PlayerStateMachine _playerPrefab;
    [SerializeField] private CinemachineVirtualCamera _cameraPrefab;
    [SerializeField] private Transform _spawnTransform;

    [Header("Sound References (Set before play mode)")]
    [SerializeField] private SoundPlayer _soundPlayer;
    [SerializeField] private MusicPlayer _musicSoundPlayer;

    [Header("DEBUG")]
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

        FXSoundPlayer = _soundPlayer;
        MusicSoundPlayer = _musicSoundPlayer;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadMenu();
    }

    private static string GetSceneName(string scenePath)
    {
        if (string.IsNullOrEmpty(scenePath)) return string.Empty;
        return Path.GetFileNameWithoutExtension(scenePath);
    }

    private void LoadMenu()
    {
        StartCoroutine(LoadMenuRoutine());
    }

    private System.Collections.IEnumerator LoadMenuRoutine()
    {
        string menuName = GetSceneName(_menuScene);
        if (!string.IsNullOrEmpty(_menuScene) && !SceneManager.GetSceneByName(menuName).isLoaded)
        {
            AsyncOperation loadOp = SceneManager.LoadSceneAsync(_menuScene, LoadSceneMode.Additive);
            yield return new WaitUntil(() => loadOp.isDone);
            _isMenuLoaded = true;
            MenuLoaded?.Invoke();
        }
        else
        {
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
        MusicSoundPlayer.PlayDefaultGlobalSound();
        InitPlayer(Instantiate(_playerPrefab, _spawnTransform.position, Quaternion.identity));
        Camera = Instantiate(_cameraPrefab, _spawnTransform.position, Quaternion.identity);
        Camera.m_Follow = Player.transform;
        _spawnTransform.gameObject.SetActive(false);

        GameStarted?.Invoke();

        string menuName = GetSceneName(_menuScene);
        if (_isMenuLoaded && !string.IsNullOrEmpty(_menuScene) && SceneManager.GetSceneByName(menuName).isLoaded)
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(_menuScene);
            yield return new WaitUntil(() => unloadOp.isDone);
            _isMenuLoaded = false;
        }

        _environmentScene = _currentLevel.LevelScenes.First();

        foreach (var scene in _currentLevel.LevelScenes)
        {
            string sceneName = GetSceneName(scene);
            if (!string.IsNullOrEmpty(scene) && !SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                AsyncOperation loadOp = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
                yield return new WaitUntil(() => loadOp.isDone);
                _levelScenes.Add(scene);
            }
            else if (!string.IsNullOrEmpty(scene))
            {
                _levelScenes.Add(scene);
            }
        }

        IsGameStarted = true;
    }

    public void MoveObjectToEnvironment(GameObject obj)
    {
        SceneHelper.MoveObjectToScene(obj, _environmentScene);
    }

    public void ReturnToMenu()
    {
        StartCoroutine(ReturnToMenuRoutine());
    }

    private System.Collections.IEnumerator ReturnToMenuRoutine()
    {
        _environmentScene = "";
        foreach (var scenePath in _levelScenes)
        {
            string sceneName = GetSceneName(scenePath);
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                yield return SceneManager.UnloadSceneAsync(scenePath);
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

        string menuName = GetSceneName(_menuScene);
        if (!string.IsNullOrEmpty(_menuScene) && !SceneManager.GetSceneByName(menuName).isLoaded)
        {
            AsyncOperation loadOp = SceneManager.LoadSceneAsync(_menuScene, LoadSceneMode.Additive);
            yield return new WaitUntil(() => loadOp.isDone);
            _isMenuLoaded = true;
        }
        else
        {
            _isMenuLoaded = true;
        }

        GameFinished?.Invoke();
        IsGameStarted = true;
    }

    public void QuitApplication()
    {
        GameFinished?.Invoke();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
