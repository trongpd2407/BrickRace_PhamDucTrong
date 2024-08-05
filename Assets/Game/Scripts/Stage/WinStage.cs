using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPos : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") ){
            GameManager.Instance.OnVictory();
        }
        if (collision.gameObject.CompareTag("Bot"))
        {
            GameManager.Instance.OnLose();
        }
    }
}
