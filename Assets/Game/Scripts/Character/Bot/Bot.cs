using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Bot : AbstractCharacter
{
    private IState<Bot> currentState;
    [SerializeField] private LayerMask layerMask;
    private Vector3 target;

    [SerializeField]
    private NavMeshAgent agent;

 
    private void Start()
    {
        if(currentState == null) {
            ChangeState(new PickBrickState());
        }
    }
    private void Update()
    {
        currentState.OnExecute(this);
      
    }
    private void FixedUpdate()
    {
    }
    public void ChangeState(IState<Bot> state)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = state;
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    public override void Move()
    {
        agent.SetDestination(target);
    }

    public void SetTarget(Vector3 newTarget)
    {
        target = newTarget;
    }

  
    public void PickBrick()
    {
        Collider[] colliders = new Collider[5];
        int numCollider = Physics.OverlapSphereNonAlloc(transform.position, 2f, colliders, layerMask) ;
        float minDis = float.MaxValue;
        for(int i = 0; i < numCollider; i++)
        {
            float distance = Vector3.Distance(colliders[i].transform.position, transform.position);
            if ( distance < minDis)
            {
                minDis = distance;
                SetTarget(colliders[i].transform.position);
            }
        }
    }

 
}

