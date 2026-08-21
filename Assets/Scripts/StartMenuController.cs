using UnityEngine;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    private GlobalData globalData;
    [SerializeField] Button playButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button quitButton;
    void Start()
    {
        globalData = GlobalData.instance;
        playButton.onClick.AddListener(() => PlayButton_OnClick());
        settingsButton.onClick.AddListener(() => SettingsButton_OnClick());
        quitButton.onClick.AddListener(() => QuitButton_OnClick());
    }
    private void PlayButton_OnClick()
    {
        globalData.StartGame_Play();
    }
    private void SettingsButton_OnClick()
    {
        
    }
    private void QuitButton_OnClick()
    {
        Application.Quit();
    }
}
