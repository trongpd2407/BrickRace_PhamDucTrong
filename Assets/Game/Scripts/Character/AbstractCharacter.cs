using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AbstractCharacter : MonoBehaviour
{
    public const float CHARACTER_SPEED = 70f;
    public LayerMask brickLayerMask;
    protected Vector3 characterDirection;
    protected bool isFall;
    [SerializeField] protected Collider charCollider;

    protected Stack<Brick> stackBrick = new Stack<Brick>();
    [SerializeField] protected Rigidbody rb;
    [SerializeField]
    private Transform stackTF;

    [SerializeField]
    protected Animator animator;

    [SerializeField]
    private Brick brickPrefab;

    [SerializeField]
    protected SkinnedMeshRenderer skinnedMeshRenderer;

    [SerializeField]
    private LayerMask brigdeLayerMask;

    [SerializeField]
    private InGameColor charColor;


 
    public virtual void OnInIt()
    {
        stackBrick.Clear();
        SimplePool.Collect(brickPrefab);
    }

    public abstract void Move();
    public virtual Color GetColor()
    {
        return skinnedMeshRenderer.material.color;
    }
    public virtual IEnumerator SetIsFall(bool isFall)
    {
        yield return new WaitForSeconds(2f);
        this.isFall = isFall;
    }

    public virtual InGameColor GetInGameColor()
    {
        return charColor;
    }
    public virtual void StackUp(InGameColor brickColor)
    {
        int brickNum = stackBrick.Count;
        Vector3 newPos = new Vector3(0, Constants.BRICK_HEIGHT * brickNum, 0);
        Brick newBrick = SimplePool.Spawn<Brick>(brickPrefab, newPos, Quaternion.identity, stackTF);
        stackBrick.Push(newBrick);
        newBrick.TurnOffCollider();
        newBrick.SetData(brickColor);
    }

    public virtual void Fall()
    {
        foreach(Brick brick in stackBrick)
        {
            StartCoroutine(brick.FallDown());
        }
        stackTF.DetachChildren();
        stackBrick.Clear();
        isFall = true;
        animator.SetInteger(Constants.ANI_RESULT, Constants.RESULT_NOTWIN);
        animator.SetBool(Constants.ANI_ISFALL, isFall);
        rb.isKinematic = true;
        charCollider.enabled = false;
    }
    public virtual IEnumerator WakeUp()
    {
        yield return new WaitForSeconds(1f);
        animator.SetInteger(Constants.ANI_RESULT, Constants.RESULT_NOTWIN);
        animator.SetBool(Constants.ANI_ISFALL, false);
        yield return new WaitForSeconds(2.5f);
        rb.isKinematic = false;
        charCollider.enabled = true;
        isFall = false;

    }
    public int GetStackCount()
    {
        return stackBrick.Count;
    }

    public void StackDown()
    {
        SimplePool.Despawn(stackBrick.Pop());
    }

    public virtual IEnumerator TurnOffCollider() {
        charCollider.enabled = false;
        yield return new WaitForSeconds(3f);
        charCollider.enabled = true;
    }
    protected virtual void OnCollisionEnter(Collision collision)
    {
    }



}
