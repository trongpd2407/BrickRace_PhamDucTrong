using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCharacter : MonoBehaviour
{
    public const float CHARACTER_SPEED = 70f;

    protected Vector3 characterDirection;

    private bool isMoving;

    private Stack<GameUnit> stackBrick = new Stack<GameUnit>();

    [SerializeField]
    private Transform stackTF;

    [SerializeField]
    private GameUnit brickPrefab;

    [SerializeField]
    protected SkinnedMeshRenderer skinnedMeshRenderer;

    [SerializeField]
    private LayerMask brigdeLayerMask;
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
    public virtual void StackUp(InGameColor brickColor)
    {
        int brickNum = stackBrick.Count;
        Vector3 newPos = new Vector3(0, 0.06f * brickNum, 0);
        Brick newBrick = SimplePool.Spawn<Brick>(brickPrefab, newPos, Quaternion.identity, stackTF);
        stackBrick.Push(newBrick);
        newBrick.TurnOffCollider();
        newBrick.SetData(brickColor);
    }

    public int GetStackCount()
    {
        return stackBrick.Count;
    }

    public void StackDown()
    {
        SimplePool.Despawn(stackBrick.Pop());
    }


    

}
