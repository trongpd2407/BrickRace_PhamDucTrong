using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static int currentLevel;
    public static bool IsGamePaused = false;
    [SerializeField] private Transform parrent;
    private Player player;
    private Bot[] bot;
    private void Awake()
    {
        player = FindAnyObjectByType<Player>();
        bot = FindObjectsOfType<Bot>();
    }
    private void Start()
    {
        currentLevel = 1;
        UIManager.Instance.OpenUI<CanvasMainMenu>();
        LoadLevel(GetCurrentLevel());
        Pause();
    }
    public int GetCurrentLevel()
    {
        return currentLevel;
    }
    public void LoadLevel(int level)
    {
        int child = transform.childCount;
        if(child > 0)
        {
            Transform currentLevel = transform.GetChild(0);
            Destroy(currentLevel.gameObject);
        }
        GameObject prefab = Resources.Load("Level/Level " + level).GameObject();
        Instantiate(prefab, parrent);
    }
    //FIX :Cant Restart // BUG bot...
    public void Restart()
    {
        LoadLevel(GetCurrentLevel());
        player.OnInIt();
        for(int i = 0; i < bot.Length; i++)
        {
            bot[i].OnInIt();
        }
    }
    // FIX: Many BUG
    public void NextLevel()
    {
        currentLevel++;
        LoadLevel(GetCurrentLevel());
        player.OnInIt();
        for (int i = 0; i < bot.Length; i++)
        {
            bot[i].OnInIt();
            bot[i].ChangeState(new PickBrickState());
        }
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        IsGamePaused = false;
    }
    
    public void Pause()
    {
        Time.timeScale = 0f;
        IsGamePaused = true;
    }
    public void OnLose()
    {
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<CanvasFail>();
    }
    public void OnVictory()
    {
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<CanvasVictory>();
    }

    public void OnSetting()
    {
        UIManager.Instance.OpenUI<CanvasSetting>();
    }

}
