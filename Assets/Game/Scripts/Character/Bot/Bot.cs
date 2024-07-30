using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
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

    private List<Transform> listStages;

    private int currentStage;
    private int nextStage;

    private bool isFindBridge;

    private Vector3 targetBridge;

    [SerializeField] private int ATTACK_PLAYER_THRESHOLD;
    [SerializeField] private int BUILD_BRIDGE_THRESHOLD;

    private void Start()
    {
        listStages = new List<Transform>();
        GameObject level = GameObject.Find("Level");
        if (level == null) {
            Debug.LogError("Can not find level");
        }
        Transform stageTf = level.transform.Find("Stage");
        if (stageTf == null)
        {
            Debug.LogError("CanNotFindStage");
        }
        foreach (Transform child in stageTf)
        {
            listStages.Add(child);
        }
        currentStage = 0;

        if (currentState == null) {
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


    public void FindBrick()
    {
        if (Vector3.Distance(transform.position, listStages[currentStage].position) > 1.3f)
        {
            SetTarget(listStages[currentStage].position);
        }
        Collider[] colliders = new Collider[5];
        int numCollider = Physics.OverlapSphereNonAlloc(transform.position, 2f, colliders, layerMask);
        float minDis = float.MaxValue;
        for (int i = 0; i < numCollider; i++)
        {
            float distance = Vector3.Distance(colliders[i].transform.position, transform.position);
            if (distance < minDis)
            {
                minDis = distance;
                SetTarget(colliders[i].transform.position);
            }
        }
    }
    public void IncreaseStage()
    {
        currentStage++;
    }
    public void SetNextStage()
    {
        nextStage = currentStage + 1;
    }
    public Transform GetNextStage()
    {
        return listStages[nextStage];
    }
    public Vector3 GetTarget()
    {
        return target;
    }
    public Vector3 GetTargetBridge()
    {
        return targetBridge;
    }

    public int GetAttackThreshold()
    {
        return ATTACK_PLAYER_THRESHOLD;
    }

    public int GetBuildBridgeThreshold()
    {
        return BUILD_BRIDGE_THRESHOLD;
    }

    public void SetIsFindBridge(bool isFind)
    {
        isFindBridge = isFind;
    }
    public void FindBridge(Transform stage)
    {
        if (!isFindBridge)
        {
            Debug.Log(stage.name);
            float minDis = float.MaxValue;
            Transform bridge = stage.Find("Bridge");
            if (bridge == null)
            {
                Debug.LogError("Cant Find Bridge");
            }
            foreach (Transform bridgeChild in bridge)
            {
                float distance = Vector3.Distance(bridgeChild.position, transform.position);
                if (distance < minDis)
                {
                    minDis = distance;
                    targetBridge = bridgeChild.position;
                    isFindBridge = true;
                }
            }
        }
        SetTarget(targetBridge);
    }
 
}

