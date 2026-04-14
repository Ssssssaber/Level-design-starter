using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _exitGameButton;
    [SerializeField] private GlobalSoundEntry _mainMenuMusic;
    [SerializeField] private OptionsMenu _options;

    private void Start()
    {
        GameManager.Instance.MusicSoundPlayer.PlaySound(_mainMenuMusic, transform);
        _startGameButton.onClick.AddListener(UIStartGame);
        _optionsButton.onClick.AddListener(UIOptions);
        _exitGameButton.onClick.AddListener(UIExitGame);
    }

    private void UIStartGame()
    {
        GameManager.Instance.StartGame();
    }

    private void UIOptions()
    {
        _options.gameObject.SetActive(true);
    }

    private void UIExitGame()
    {
        GameManager.Instance.QuitApplication();
    }
}