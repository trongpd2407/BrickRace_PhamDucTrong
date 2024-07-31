using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCharacter : MonoBehaviour
{
    public const float CHARACTER_SPEED = 70f;
    public LayerMask brickLayerMask;
    protected Vector3 characterDirection;

    [SerializeField] protected Collider charCollider;
    private bool isMoving;

    private Stack<Brick> stackBrick = new Stack<Brick>();

    [SerializeField]
    private Transform stackTF;

    [SerializeField]
    private Brick brickPrefab;

    [SerializeField]
    protected SkinnedMeshRenderer skinnedMeshRenderer;

    [SerializeField]
    private LayerMask brigdeLayerMask;

    [SerializeField]
    private InGameColor charColor;
    public void OnInIt()
    {
        stackBrick.Clear();
    }
    public bool IsMoving
    {
        get
        {
            return isMoving;
        }
    }
    public abstract void Move();
    public virtual Color GetColor()
    {
        return skinnedMeshRenderer.material.color;
    }

    public virtual InGameColor GetInGameColor()
    {
        return charColor;
    }
    public virtual void StackUp(InGameColor brickColor)
    {
        int brickNum = stackBrick.Count;
        Vector3 newPos = new Vector3(0, 0.06f * brickNum, 0);
        Brick newBrick = SimplePool.Spawn<Brick>(brickPrefab, newPos, Quaternion.identity, stackTF);
        stackBrick.Push(newBrick);
        newBrick.TurnOffCollider();
        newBrick.SetData(brickColor);
    }
    private void Update()
    {
       
    }
    public virtual void Fall()
    {
        foreach(Brick brick in stackBrick)
        {
            StartCoroutine(brick.FallDown());
        }
        stackTF.DetachChildren();
        stackBrick.Clear();
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
