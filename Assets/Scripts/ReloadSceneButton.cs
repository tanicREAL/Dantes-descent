using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReloadSceneButton : MonoBehaviour
{
    public Button restartButton;
    public Text buttonText;

    void Start()
    {
        // Get references to the Button and Text components
        restartButton = GetComponent<Button>();
        buttonText = restartButton.GetComponentInChildren<Text>();

        // Subscribe to the button's onClick event
        restartButton.onClick.AddListener(ReloadScene);

        // Optionally, update the button text
        UpdateButtonText("Restart");
    }

    void ReloadScene()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpdateButtonText(string newText)
    {
        // Update the button text
        if (buttonText != null)
        {
            buttonText.text = newText;
        }
    }
}
