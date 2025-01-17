using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Bridge : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private BoxCollider boxCollider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAG_PLAYER) )
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
        if (other.CompareTag(Constants.TAG_BOT))
        {
            Bot bot = other.GetComponent<Bot>();
            if (bot == null)
            {
                return;
            }
            if (bot.GetStackCount() > 0 && meshRenderer.material.color != bot.GetColor())
            {
                boxCollider.isTrigger = true;
                bot.StackDown();
                meshRenderer.material.color = bot.GetColor();
            }
            else if (meshRenderer.material.color == bot.GetColor())
            {
                boxCollider.isTrigger = true;
            }
            else if(bot.GetStackCount() <= 0)
            {
                boxCollider.isTrigger = false;
                bot.ChangeState(new PickBrickState());
                
            }

        }



    }
}
