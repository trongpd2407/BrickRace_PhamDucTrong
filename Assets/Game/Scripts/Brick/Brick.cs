using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : GameUnit
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private Rigidbody rb;
    private InGameColor brickColor;
    private BrickSO brickSO;


    public void OnInIt()
    {
        boxCollider.enabled = true;
        meshRenderer.enabled = true;
    }
    public void OnRespawn()
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
    public InGameColor GetBrickIGC()
    {
        return brickColor;
    }
    public void TurnOffCollider()
    {
        boxCollider.enabled = false;
    }

    public IEnumerator FallDown()
    {
        rb.isKinematic = false;
        boxCollider.isTrigger = false;
        rb.useGravity = true;
        boxCollider.enabled = true;
        this.gameObject.layer = LayerMask.NameToLayer("Gray");
        meshRenderer.material.color = Color.gray;
        yield return new WaitForSeconds(1f);
        rb.isKinematic = true;
        boxCollider.isTrigger = true;
        rb.useGravity = false;
    }
  
    private void OnTriggerEnter(Collider other)
    {
        AbstractCharacter character = other.GetComponent<AbstractCharacter>();
        if(character == null) {
            return;
        }
        if ((character.CompareTag("Player") || character.CompareTag("Bot"))&& character.GetColor() == meshRenderer.sharedMaterial.color){
            character.StackUp(brickColor);
            OnRespawn();
        }
        if ((character.CompareTag("Player") || character.CompareTag("Bot")) &&  meshRenderer.sharedMaterial.color == Color.gray)
        {
            character.StackUp(character.GetInGameColor());
            SimplePool.Despawn(this);
        }
    }





} 
