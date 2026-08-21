
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DTT.UI.ProceduralUI;
using PrimeTween;
using System.Collections.Generic;
using Unity.Collections;
using System.Linq;
using DTT.Utils.Extensions;
using System.Threading.Tasks;

public class Tweenz4Button : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
{
    [SerializeField] float tweenDuration = 0.25f;
    private Vector3 defaultScale;


    [Space(20)]
    public Image iconImage;
    private Color defaultIconColor;
    [SerializeField] Color activeIconColor = new Color(0.7529f, 0.3882f, 0.9451f);
    [SerializeField] bool activeIcon_OnHover = true;
    public bool activeIcon_Keep_OffHover = false;
    private Tween tweenIcon;


    [Space(20)]
    public TMP_Text textComponent;
    private Color defaultTextColor;
    [SerializeField] Color activeTextColor = new Color(0.7529f, 0.3882f, 0.9451f);
    [SerializeField] bool activeText_OnHover = true;
    private Tween tweenText;


    [Space(20)]
    public RoundedImage mainBackground;
    private Color defaultMainBackgroundColor;
    [SerializeField] Color activeMainBackgroundColor;
    private Tween tweenMainBackground;
    private Tween tweenMainBackgroundFlash;
    // These are basically for category buttons only
    private float defaultMainBackgroundGradientBottomOffset;
    private Tween tweenMainBackgroundGradientBottomOffset;
    public RoundedImage mainBackgroundGradient;
    private Tween tweenMainBackgroundGradient;
    ///////////////////////////////////////////////////


    [Space(20)]
    public RoundedImage secondaryBackground;
    private Color defaultSecondaryBackgroundColor;
    [SerializeField] Color activeSecondaryBackgroundColor = new Color(0.7020f, 0.1216f, 1.0f);
    [SerializeField] bool activeSecondary_OnHover = true;
    private Tween tweenSecondaryBackground;


    [Space(20)]
    [SerializeField] bool scaleUp_OnClick = false;
    [SerializeField] float scalePow = 1.25f;
    private Tween tweenScale;


    [Space(20)]
    [SerializeField] bool flashWhite_OnClick = true;
    [SerializeField] float flashDurationMultiplier = 0.5f;
    public bool isSelected = false;

    void Awake()
    {
        defaultScale = transform.localScale;
        if (iconImage != null)
            defaultIconColor = iconImage.color;
        if (textComponent != null)
            defaultTextColor = textComponent.color;
        if (mainBackground != null)
        {
            defaultMainBackgroundColor = mainBackground.color;
        }
        if (mainBackgroundGradient != null)
        {
            defaultMainBackgroundGradientBottomOffset = mainBackgroundGradient.GetRectTransform().offsetMin.y;
        }
        if (secondaryBackground != null)
            defaultSecondaryBackgroundColor = secondaryBackground.color;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (scaleUp_OnClick == true)
            Tween_ScaleUp();
        if (flashWhite_OnClick == true)
            Tween_FlashWhite(flashDurationMultiplier);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (activeSecondary_OnHover == true)
            Tween_SecondaryBackground(true);
        if (activeText_OnHover == true)
            Tween_TextColor(true, 1f);
        if (activeIcon_OnHover == true)
            Tween_IconColor(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (activeSecondary_OnHover == true)
            Tween_SecondaryBackground(false);
        if (activeText_OnHover == true)
            Tween_TextColor(false, 1f);
        if (activeIcon_OnHover == true && activeIcon_Keep_OffHover == false)
            Tween_IconColor(false);
    }
    void OnDisable()
    {
        if (tweenScale.isAlive)
            tweenScale.Complete();
        if (tweenIcon.isAlive)
            tweenIcon.Complete();
        if (tweenText.isAlive)
            tweenText.Complete();
        if (tweenMainBackground.isAlive)
            tweenMainBackground.Complete();
        if (tweenMainBackgroundGradientBottomOffset.isAlive)
            tweenMainBackgroundGradientBottomOffset.Complete();
        if (tweenMainBackgroundGradient.isAlive)
            tweenMainBackgroundGradient.Complete();
        if (tweenSecondaryBackground.isAlive)
            tweenSecondaryBackground.Complete();

        if (iconImage != null && iconImage.color != defaultIconColor)
            iconImage.color = defaultIconColor;
        if (textComponent != null && textComponent.color != defaultTextColor)
            textComponent.color = defaultTextColor;
        if (mainBackground != null && mainBackground.color != defaultMainBackgroundColor)
            mainBackground.color = defaultMainBackgroundColor;
        if (secondaryBackground != null && secondaryBackground.color != defaultSecondaryBackgroundColor)
            secondaryBackground.color = defaultSecondaryBackgroundColor;
    }
    public void Tween_FlashWhite(float durationMultiplier = 0.5f)
    {
        if (mainBackground == null)
            return;
        if (tweenMainBackgroundFlash.isAlive == true)
            tweenMainBackgroundFlash.Complete();
        var startColor = defaultMainBackgroundColor;
        var tweenSequence = Sequence.Create(useUnscaledTime: true);
        tweenSequence.Chain(
            Tween.Color(
                mainBackground,
                Color.white,
                tweenDuration / 2 * durationMultiplier
            )
        );
        tweenSequence.Chain(
            Tween.Color(
                mainBackground,
                startColor,
                tweenDuration / 2 * durationMultiplier
            )
        );
    }
    public void Tween_IconColor(bool toActive)
    {
        if (iconImage == null)
            return;
        var settings = new TweenSettings<Color>();
        if (toActive == true)
            settings.endValue = activeIconColor;
        else
            settings.endValue = defaultIconColor;

        if (settings.endValue == iconImage.color)
            return;

        settings.startFromCurrent = true;
        settings.settings.duration = tweenDuration;
        settings.settings.useUnscaledTime = true;

        tweenIcon = Tween.Color(iconImage, settings);
    }
    public void Tween_TextColor(bool toActive, float speedPow = 1f)
    {
        if (textComponent == null)
            return;
        var settings = new TweenSettings<Color>();
        if (toActive == true)
            settings.endValue = activeTextColor;
        else
            settings.endValue = defaultTextColor;

        if (settings.endValue == textComponent.color)
            return;

        settings.startFromCurrent = true;
        settings.settings.duration = tweenDuration * speedPow;
        settings.settings.useUnscaledTime = true;

        tweenText = Tween.Color(textComponent, settings);
    }
    public void Tween_SecondaryBackground(bool toActive)
    {
        if (secondaryBackground == null)
            return;
        
        var settings = new TweenSettings<Color>();
        if (toActive == true)
        {
            settings.startValue = defaultSecondaryBackgroundColor;
            settings.endValue = activeSecondaryBackgroundColor;
        }
        else
        {
            settings.startValue = activeSecondaryBackgroundColor;
            settings.endValue = defaultSecondaryBackgroundColor;
        }
        settings.settings.duration = tweenDuration;
        settings.settings.useUnscaledTime = true;

        tweenSecondaryBackground = Tween.Color(secondaryBackground, settings);
    }
    #region CategoryButton stuff
    public void Tween_ScaleUp()
    {
        if (tweenScale.isAlive == true)
            tweenScale.Complete();
        if (transform.localScale != defaultScale)
            return;
        var settings = new TweenSettings<Vector3>();
        settings.endValue = Vector3.one * scalePow;
        settings.startFromCurrent = true;
        settings.settings.duration = tweenDuration;
        settings.settings.useUnscaledTime = true;

        tweenScale = Tween.Scale(transform, settings);
    }
    public void Tween_ScaleDown()
    {
        if (tweenScale.isAlive == true)
            tweenScale.Complete();
        if (transform.localScale == defaultScale)
            return;
        var settings = new TweenSettings<Vector3>();
        settings.endValue = Vector3.one;
        settings.startFromCurrent = true;
        settings.settings.duration = tweenDuration;
        settings.settings.useUnscaledTime = true;

        tweenScale = Tween.Scale(transform, settings);
    }
    public void Tween_MainBackgroundGradientBottomOffset(bool toSelected)
    {
        if (mainBackgroundGradient == null)
            return;
        if (tweenMainBackgroundGradientBottomOffset.isAlive == true)
            tweenMainBackgroundGradientBottomOffset.Complete();

        var settings = new TweenSettings<float>();
        if (toSelected == true)
        {
            settings.startValue = defaultMainBackgroundGradientBottomOffset;
            settings.endValue = defaultMainBackgroundGradientBottomOffset - 10f;
        }
        else
        {
            if (mainBackgroundGradient.GetRectTransform().offsetMin.y == defaultMainBackgroundGradientBottomOffset)
                return;
            settings.startValue = defaultMainBackgroundGradientBottomOffset - 10f;
            settings.endValue = defaultMainBackgroundGradientBottomOffset;
        }
        settings.settings.ease = Ease.OutExpo;
        settings.settings.duration = tweenDuration;
        settings.settings.useUnscaledTime = true;

        tweenMainBackgroundGradientBottomOffset = Tween.UIOffsetMinY
            (mainBackgroundGradient.GetRectTransform(), settings);
    }
    public void Tween_MainBackgroundAlpha(bool toSelected)
    {
        if (mainBackground == null)
            return;
        if (tweenMainBackgroundGradient.isAlive == true)
            tweenMainBackgroundGradient.Complete();
        var settings = new TweenSettings<Color>();
        settings.endValue = defaultMainBackgroundColor;
        settings.startValue = defaultMainBackgroundColor;
        if (toSelected == true)
        {
            settings.startValue.a = 1f;
            settings.endValue.a = 0f;
        }
        else
        {
            settings.startValue.a = 0f;
            settings.endValue.a = 1f;
        }
        settings.settings.ease = Ease.OutExpo;
        settings.settings.duration = tweenDuration;
        settings.settings.useUnscaledTime = true;

        tweenMainBackgroundGradient = Tween.Color(mainBackground, settings);
    }
    #endregion
}
