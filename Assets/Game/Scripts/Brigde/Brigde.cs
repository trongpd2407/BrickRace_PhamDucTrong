using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Brigde : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private BoxCollider boxCollider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") )
        {
            Player player = other.GetComponent<Player>();
            if (player == null)
            {
                return;
            }
            if (player.GetStackCount() > 0 && meshRenderer.material.color != player.GetColor())
            {
                boxCollider.isTrigger = true;
                player.StackDown();
                meshRenderer.material.color  = player.GetColor();
            }
            else if (meshRenderer.material.color == player.GetColor())
            {
                boxCollider.isTrigger = true;
            }
            else
            {
                boxCollider.isTrigger = false;
            }
        }
        if (other.CompareTag("Bot"))
        {
            Bot bot = other.GetComponent<Bot>();
            if (bot == null)
            {
                return;
            }
            if (bot.GetStackCount() > 0 && meshRenderer.material.color != bot.GetColor())
            {
                bot.StackDown();
                meshRenderer.material.color = meshRenderer.material.color = bot.GetColor();
            }
        }



    }
}
