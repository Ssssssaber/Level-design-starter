using UnityEngine;
using UnityEngine.UI;

using System;
using TMPro;

public class GameResultMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menuParent;
    [SerializeField] private Button _returnToMenu;
    [SerializeField] private Button _exitGameButton;
    [SerializeField] private TMP_Text _resultText;


    [Serializable]
    public class GameResultUI
    {
        public string Text;
        public GlobalSoundEntry Music;
    }

    [SerializeField] private GameResultUI _gameLost;
    [SerializeField] private GameResultUI _gameWon;


    private void Start()
    {
        GameManager.Instance.GameWon += OnGameWon;
        GameManager.Instance.GameLost += OnGameLost;
       GameManager.Instance.GameFinished += DisableMenu;

        _returnToMenu.onClick.AddListener(ReturnToMenu);
        _exitGameButton.onClick.AddListener(UIExitGame);
    }

    public void EnableMenu()
    {
        _menuParent.SetActive(true);
    }

    public void DisableMenu()
    {
        _menuParent.SetActive(false);
    }

    public void OnGameLost()
    {
        EnableMenu();
        HandleGameResult(_gameLost);
    }


    public void OnGameWon()
    {
        EnableMenu();
        HandleGameResult(_gameWon);
    }

    private void HandleGameResult(GameResultUI resultUI)
    {
        GameManager.Instance.MusicSoundPlayer.PlaySound(resultUI.Music, transform);
        _resultText.text = resultUI.Text;
    }

    private void ReturnToMenu()
    {
        GameManager.Instance.ReturnToMenu();
    }

    private void UIExitGame()
    {
        GameManager.Instance.QuitApplication();
    }
}