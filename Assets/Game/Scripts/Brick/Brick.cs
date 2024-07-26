using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : GameUnit
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private BoxCollider boxCollider;
    private InGameColor brickColor;
    private BrickSO brickSO;


    public void OnInIt()
    {
        boxCollider.enabled = true;
        meshRenderer.enabled = true;
    }
    public void OnDespawn()
    {
        if (boxCollider.enabled)
        {
            boxCollider.enabled = false;
        }
        if (meshRenderer.enabled)
        {
            meshRenderer.enabled = false;
        }
        Invoke(nameof(OnInIt), 3f);
    }
    private void Update()
    {
    }


    public void SetData(InGameColor brickColor)
    {
        this.brickColor = brickColor;
        switch (brickColor)
        {
            case InGameColor.Red:
                brickSO = Resources.Load<BrickSO>(Constants.BRICK_RED_SO_PATH);
                break;
            case InGameColor.Blue:
                brickSO = Resources.Load<BrickSO>(Constants.BRICK_BLUE_SO_PATH);
                break;
            case InGameColor.Green:
                brickSO = Resources.Load<BrickSO>(Constants.BRICK_GREEN_SO_PATH);
                break;
            case InGameColor.Yellow:
                brickSO = Resources.Load<BrickSO>(Constants.BRICK_YELLOW_SO_PATH);
                break;
        }
        meshRenderer.sharedMaterial = brickSO.material;
        gameObject.layer = brickSO.layerMask;
    }
    public void TurnOffCollider()
    {
        boxCollider.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        AbstractCharacter character = other.GetComponent<AbstractCharacter>();
        if(character == null) {
            return;
        }
        if ((character.CompareTag("Player") || character.CompareTag("Bot"))&& character.GetColor() == meshRenderer.sharedMaterial.color){
            character.StackUp(brickColor);
            OnDespawn();
        }
    }

} 
