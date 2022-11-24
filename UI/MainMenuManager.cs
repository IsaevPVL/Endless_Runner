using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void SetControlSchemeTap()
    {
        PlayerControl.currentScheme = ControlScheme.Tap;
    }
    public void SetControlSchemeSwipe()
    {
        PlayerControl.currentScheme = ControlScheme.Swipe;
    }
    public void SetControlSchemeDelta()
    {
        PlayerControl.currentScheme = ControlScheme.DeltaSwipe;
    }
}
