using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    private GlobalData globalData;
    [Header("EscMenu Stuff")] 
    [SerializeField] GameObject escMenuObj;
    [SerializeField] Button escMenu_ContinueButton;
    [SerializeField] Button escMenu_SettingsButton;
    [SerializeField] Button escMenu_QuitButton;
    void Start()
    {
        globalData = GlobalData.instance;
        escMenu_ContinueButton.onClick.AddListener(() => EscMenu_ContinueButton_OnClick());
        escMenu_SettingsButton.onClick.AddListener(() => EscMenu_SettingsButton_OnClick());
        escMenu_QuitButton.onClick.AddListener(() => EscMenu_QuitButton_OnClick());
    }
    void Update()
    {
        CheckInput();
    }
    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetActiveOpposite_EscMenu();
        }
    }
    private void SetActiveOpposite_EscMenu()
    {
        escMenuObj.SetActive(!escMenuObj.activeSelf);
        globalData.playerController.SetAllowInput(!escMenuObj.activeSelf);
    }
    private void EscMenu_ContinueButton_OnClick()
    {
        SetActiveOpposite_EscMenu();
    }
    private void EscMenu_SettingsButton_OnClick()
    {
        
    }
    private void EscMenu_QuitButton_OnClick()
    {
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }
}
