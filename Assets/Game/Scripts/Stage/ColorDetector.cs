using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColorDetector : MonoBehaviour
{
    [SerializeField] Stage stage;

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag(Constants.TAG_BOT) || collision.gameObject.CompareTag(Constants.TAG_PLAYER))
        {
            List<Brick> bricks = stage.GetBricks();
            if(bricks.Count <= 0 || stage == null) {
                return;
            }
            AbstractCharacter character = collision.gameObject.GetComponent<AbstractCharacter>();
            foreach (Brick brick in bricks)
            {
                if (brick.GetBrickIGC() == character.GetInGameColor())
                {
                    brick.OnInIt();
                }
            }
        }
    }
}
