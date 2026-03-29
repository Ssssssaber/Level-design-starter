using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Button _startGameButton;
    [SerializeField] Button _optionsButton;
    [SerializeField] Button _exitGameButton;

    private void Start()
    {
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
        
    }

    private void UIExitGame()
    {
        GameManager.Instance.QuitApplication();
    }
}