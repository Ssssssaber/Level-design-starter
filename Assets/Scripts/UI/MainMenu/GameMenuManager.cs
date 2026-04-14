using UnityEngine;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _parentObject;
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private Button _returnToGameButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _exitGameButton;

    private void Start()
    {
        GameManager.Instance.GameMenuRequested += EnableGameMenu;

        _returnToGameButton.onClick.AddListener(DisableMenu);
        _optionsButton.onClick.AddListener(UIOptions);
        _mainMenuButton.onClick.AddListener(ReturnToMenu);
        _exitGameButton.onClick.AddListener(UIExitGame);
    }

    public void EnableGameMenu()
    {
        if (!GameManager.Instance.IsGameStarted) return;

        _parentObject.SetActive(true);
    }

    public void DisableMenu()
    {
        _parentObject.SetActive(false);
    }

    private void UIOptions()
    {
        _optionsMenu.SetActive(true);
    }

    private void ReturnToMenu()
    {
        DisableMenu();
        GameManager.Instance.ReturnToMenu();
    }

    private void UIExitGame()
    {
        GameManager.Instance.QuitApplication();
    }
}