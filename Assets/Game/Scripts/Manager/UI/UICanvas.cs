using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [SerializeField] bool isDestroyOnClose = false;
    private void Awake()
    {
        RectTransform rect = GetComponent<RectTransform>();
        float raito = (float) Screen.width/ (float) Screen.height;
        if(raito > 2.1)
        {
            Vector2 leftBottom = rect.offsetMin;
            Vector2 rightTop = rect.offsetMax;
            leftBottom.y = 0f;
            rightTop.y = -100f;
            rect.offsetMin = leftBottom;
            rect.offsetMax = rightTop;
        }
        
    }
    public virtual void SetUp()
    {

    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close(float time)
    { 
        Invoke(nameof(CloseDirectly), time);
    } 

    public virtual void CloseDirectly()
    {
        if(isDestroyOnClose)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }
}
