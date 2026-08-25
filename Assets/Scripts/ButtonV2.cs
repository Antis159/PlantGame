using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class ButtonV2 : Button
{
    public Tweenz4Button tweenz4Button;
    protected override void Start()
    {
        base.Start();
        TryGetComponent<Tweenz4Button>(out tweenz4Button);
        if (tweenz4Button == null)
            Debug.LogWarning($"{name} - prob should not be ButtonV2", this);
    }
}
