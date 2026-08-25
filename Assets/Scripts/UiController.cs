using System.Collections.Generic;
using System.Linq;
using DTT.Utils.Extensions;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    private GlobalData globalData;
    [Header("Category Button - Kvp")]
    [SerializeField] GlobalData.KeyValuePairV2<Button, RectTransform> visibilityButton_Kvp;
    private Tween visibilityTween;
    private bool isVisibleMainUi = true;
    [SerializeField] List<GlobalData.KeyValuePairV2<ButtonV2, GameObject>> categoryButton_ListKvp;
    [Header("EscMenu Stuff")] 
    [SerializeField] GameObject escMenuObj;
    [SerializeField] Button escMenu_ContinueButton;
    [SerializeField] Button escMenu_SettingsButton;
    [SerializeField] Button escMenu_QuitButton;
    void Start()
    {
        globalData = GlobalData.instance;
        visibilityButton_Kvp.Key.onClick.AddListener(() => VisibilityButton_OnClick());
        categoryButton_ListKvp.ForEach( x => x.Key.onClick.AddListener(() => UiCategoryListButton_OnClick(x)));

        escMenu_ContinueButton.onClick.AddListener(() => EscMenu_ContinueButton_OnClick());
        escMenu_SettingsButton.onClick.AddListener(() => EscMenu_SettingsButton_OnClick());
        escMenu_QuitButton.onClick.AddListener(() => EscMenu_QuitButton_OnClick());
        
        UiCategoryListButton_OnClick(categoryButton_ListKvp.First());
        VisibilityButton_OnClick();
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
    private void ForceVisibility_True()
    {
        if (isVisibleMainUi == false)
            VisibilityButton_OnClick();
    }
    private void UiCategoryListButton_OnClick(GlobalData.KeyValuePairV2<ButtonV2, GameObject> kvp)
    {
        ForceVisibility_True();
        foreach (var item in categoryButton_ListKvp)
        {
            if (item == kvp)
            {
                item.Key.tweenz4Button.isSelected = true;
                item.Value.SetActive(true);
                //item.Key.tweenz4Button.Tween_ScaleUp();
                item.Key.tweenz4Button.Tween_MainBackgroundGradientBottomOffset(true);
                item.Key.tweenz4Button.Tween_MainBackgroundAlpha(true);
            }
            else
            {
                item.Value.SetActive(false);
                if (item.Key.tweenz4Button.isSelected == true)
                {
                    item.Key.tweenz4Button.isSelected = false;
                    //item.Key.tweenz4Button.Tween_ScaleDown();
                    item.Key.tweenz4Button.Tween_MainBackgroundGradientBottomOffset(false);
                    item.Key.tweenz4Button.Tween_MainBackgroundAlpha(false);
                }
            }
        }
    }
    private void VisibilityButton_OnClick()
    {
        if (visibilityTween.isAlive == true)
            return;
        var tSettings = new TweenSettings<float>();
        tSettings.settings.ease = Ease.Linear;
        tSettings.settings.duration = 0.25f;
        tSettings.startFromCurrent = true;
        tSettings.settings.useUnscaledTime = true;
        if (isVisibleMainUi == true)
        {
            tSettings.endValue = visibilityButton_Kvp.Value.anchoredPosition.y - 302f;
        }
        else
        {
            tSettings.endValue = visibilityButton_Kvp.Value.anchoredPosition.y + 302f;
        }
        visibilityTween = Tween.UIAnchoredPositionY(visibilityButton_Kvp.Value, tSettings);
        isVisibleMainUi = !isVisibleMainUi;

        // TODO: prob useless if Icon is 'X'
        // This is maybe too caveman, works tho
        // var icon = visibilityButton_Kvp.Key.transform.LastChild();
        // var iconRot = icon.eulerAngles;
        // iconRot.z -= 180;
        // icon.eulerAngles = iconRot;
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
