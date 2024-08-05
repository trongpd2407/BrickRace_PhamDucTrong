using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    Dictionary<System.Type, UICanvas> canvasActives = new Dictionary<System.Type, UICanvas>();
    Dictionary<System.Type, UICanvas> canvasPrefabs = new Dictionary<System.Type, UICanvas>();
    [SerializeField] Transform parrent;
    [SerializeField] GameObject joystick;


    private void Awake()
    {
        UICanvas[] prefabs = Resources.LoadAll<UICanvas>("UI/");
        for(int i = 0; i < prefabs.Length; i++)
        {
            canvasPrefabs.Add(prefabs[i].GetType(), prefabs[i]);
        }
  
    }

    public T OpenUI<T> () where T : UICanvas
    {
        T canvas = GetUI<T>();
        canvas.SetUp();
        canvas.Open();
        if(canvas is CanvasGameplay)
        {
            joystick.SetActive(true);
            GameManager.Instance.Resume();
        }
        else
        {
            joystick.SetActive(false);  
        }
        return canvas;
    }
    public void EnableJoyStick()
    {
        joystick.SetActive( true);
    }
    public void CloseUI<T> (float time) where T : UICanvas
    {
        if(IsUIOpened<T>())
        {
            canvasActives[typeof(T)].Close(time);
        }
    }

    public void CloseUIDirectly<T>() where T : UICanvas
    {

        if (IsUIOpened<T>())
        {
            canvasActives[typeof(T)].CloseDirectly();
        }
    }
    public bool IsUILoaded<T> () where T: UICanvas
    {
        return canvasActives.ContainsKey(typeof(T)) && canvasActives[typeof(T)] != null ;
    }
    public bool IsUIOpened<T>() where T : UICanvas
    {
        return IsUILoaded<T>() && canvasActives[typeof(T)].gameObject.activeSelf;
    }

    public T GetUI<T>() where T : UICanvas
    {
        if (!IsUILoaded<T>())
        {
            T prefab = GetUIPrefab<T>();
            T canvas = Instantiate(prefab,parrent);
            canvasActives[typeof(T)] =canvas;
        }
        return canvasActives[typeof(T)] as T;
    }

    private T GetUIPrefab<T>() where T: UICanvas 
    {
        return canvasPrefabs[typeof(T)] as T;

    }
    public void CloseAll()
    {
        foreach (var t in canvasActives)
        {
            if (t.Value != null && t.Value.gameObject.activeSelf)
            {
                t.Value.CloseDirectly();
            }
        }

    }
}
