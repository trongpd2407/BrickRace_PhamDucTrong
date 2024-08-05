using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasVictory : UICanvas
{

    public void RestartButton()
    {
        Close(0);
        GameManager.Instance.Restart();
        UIManager.Instance.OpenUI<CanvasGameplay>();
    }
    public void NextLevelButton()
    {
        Close(0);
        GameManager.Instance.NextLevel();
        UIManager.Instance.OpenUI<CanvasGameplay>();
    }
    public void MainMenuButton()
    {
        Close(0);
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }
}
