using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Bot : AbstractCharacter
{
    private IState<Bot> currentState;
    private int currentLevel;
    private Vector3 target;
    [SerializeField]
    private LayerMask characterLayerMask;
    [SerializeField]
    private NavMeshAgent agent;

    private Vector3 startPos;
    private List<Transform> listStages;

    private int currentStage;
    private int nextStage;

    private bool isFoundBridge;

    private Vector3 targetBridge;

    [SerializeField] private int ATTACK_PLAYER_THRESHOLD;
    [SerializeField] private int BUILD_BRIDGE_THRESHOLD;

    private void Awake()
    {
        startPos = transform.position;
    }
    private void Start()
    {
        OnInIt();
    }
    public override void OnInIt()
    {
        base.OnInIt();
        agent.enabled = false;
        agent.transform.position = startPos;
        agent.enabled = true;
        currentLevel = GameManager.Instance.GetCurrentLevel();
        listStages = new List<Transform>();
        GameObject level = GameObject.Find("Level");
        if (level == null)
        {
            Debug.LogError("Can not find level");
        }
        Transform stageTf = level.transform.Find("Level " + currentLevel+"(Clone)");
        if (stageTf == null)
        {
            Debug.LogError("CanNotFindStage");
        }
        foreach (Transform child in stageTf)
        {
            listStages.Add(child);
        }
        currentStage = 0;
        if (currentState == null)
        {
            ChangeState(new PickBrickState());
        }
    }
    private void Update()
    {
        currentState.OnExecute(this);
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
        animator.SetFloat("velocity", agent.speed);
    }

    public void SetTarget(Vector3 newTarget)
    {
        target = newTarget;
    }


    public void FindBrick()
    {
        if (listStages[currentStage] == null)
        {
            return;
        }
        if (Vector3.Distance(transform.position, listStages[currentStage].position) > 2f )
        {
            SetTarget(listStages[currentStage].position);
            return;
        }
        Collider[] colliders = new Collider[5];
        int numCollider = Physics.OverlapSphereNonAlloc(transform.position, 3f, colliders, brickLayerMask);
        float minDis = float.MaxValue;
        Vector3 newTarget = new Vector3();
        for (int i = 0; i < numCollider; i++)
        {
          
            float distance = Vector3.Distance(colliders[i].transform.position, transform.position);
            if (distance < minDis)
            {
                minDis = distance;
                newTarget = colliders[i].transform.position;
            }

        }
        SetTarget(newTarget);
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
        isFoundBridge = isFind;
    }
    public void FindBridge(Transform stage)
    {
        if (!isFoundBridge)
        {
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
                    isFoundBridge = true;
                }
            }
        }
        SetTarget(targetBridge);
    }

    public void FindCharacter()
    {
        Collider[] colliders = new Collider[5];
        int numCollider = Physics.OverlapSphereNonAlloc(transform.position, 2f, colliders, characterLayerMask);
        if(numCollider <= 1)
        {
            ChangeState(new PickBrickState());
            return;
        }

        float minDis = float.MaxValue;
        Vector3 newTarget = new Vector3();
        for (int i = 0; i < numCollider; i++)
        {
            if (!colliders[i].gameObject.name.Equals( this.gameObject.name) && (Mathf.Abs(colliders[i].transform.position.y - this.transform.position.y) < 0.05f))
            {
                float distance = Vector3.Distance(colliders[i].transform.position, transform.position);
                if (distance < minDis)
                {
                    minDis = distance;
                    newTarget = colliders[i].transform.position;
                }
            }
         
        }
        if(Vector3.Distance(transform.position, newTarget) > 1f) {
            SetIsFindBridge(false);
            ChangeState(new PickBrickState());
            return;
        }
        SetTarget(newTarget);
    }
 
    public bool GetIsFall()
    {
        return isFall;
    }
    public override void Fall()
    {
        base.Fall();
        ChangeState(new WakeUpState());
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.CompareTag("Bot"))
        {
            Bot bot = collision.gameObject.GetComponent<Bot>();
            if (bot.GetStackCount() > this.GetStackCount())
            {
                this.ChangeState(new FallState());
                bot.ChangeState(new PickBrickState());
            }
            else if (bot.GetStackCount() < this.GetStackCount())
            {
                bot.ChangeState(new FallState());
                this.ChangeState(new PickBrickState());
            }
            else if(bot.GetStackCount() == this.GetStackCount() && bot.GetStackCount() != 0)
            {
                bot.ChangeState(new FallState());
                this.ChangeState(new FallState());
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player.GetStackCount() > this.GetStackCount())
            {
                this.ChangeState(new FallState());
            }
            else if (player.GetStackCount() < this.GetStackCount())
            {
                player.Fall();
                this.ChangeState( new PickBrickState());
            }
            else
            {
                player.Fall();
                this.ChangeState(new FallState());
            }
        }
    }

}

