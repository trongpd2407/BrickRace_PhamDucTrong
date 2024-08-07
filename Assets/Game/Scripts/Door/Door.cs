using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform leftDoor;
    [SerializeField] private Transform rightDoor;
    [SerializeField] private BoxCollider boxCollider;

    public void OnOpen()
    {
        leftDoor.position = Vector3.MoveTowards(leftDoor.position,new Vector3(leftDoor.position.x - 0.3f,leftDoor.position.y, leftDoor.position.z), 0.5f);
        rightDoor.position = Vector3.MoveTowards(rightDoor.position, new Vector3(rightDoor.position.x + 0.3f, rightDoor.position.y, rightDoor.position.z), 0.5f);
    }

    public void OnClose() 
    {
        leftDoor.position = Vector3.MoveTowards(leftDoor.position, new Vector3(leftDoor.position.x + 0.3f, leftDoor.position.y, leftDoor.position.z), 0.5f);
        rightDoor.position = Vector3.MoveTowards(rightDoor.position, new Vector3(rightDoor.position.x - 0.3f, rightDoor.position.y, rightDoor.position.z), 0.5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        OnOpen();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.TAG_BOT))
        {
            Bot bot = other.GetComponent<Bot>();
            bot.IncreaseStage();
            bot.SetIsFindBridge(false);
            bot.ChangeState(new PickBrickState());
        }
        OnClose();
    }
}
