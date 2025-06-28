using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    private bool isGameOver = false;
    private InputAction restartAction;

    private void OnEnable()
    {
        restartAction = new InputAction(binding: "<Keyboard>/r");
        restartAction.Enable();
    }

    private void OnDisable()
    {
        restartAction.Disable();
    }

    void Update()
    {
        if (isGameOver && restartAction.triggered)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void ShowGameOver()
    {
        gameOverScreen.SetActive(true);
        isGameOver = true;
    }
}
