using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGameplay : UICanvas
{
    public void SettingButton()
    {
        GameManager.Instance.Pause();
        UIManager.Instance.OpenUI<CanvasSetting>().SetState(this);
    }
}
