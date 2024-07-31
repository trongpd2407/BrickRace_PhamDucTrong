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

    private Vector3 target;
    [SerializeField]
    private LayerMask characterLayerMask;

    [SerializeField]
    private NavMeshAgent agent;

    private List<Transform> listStages;

    private int currentStage;
    private int nextStage;

    private bool isFoundBridge;

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
        Transform stageTf = level.transform.Find("Level 1");
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
        Debug.Log(currentState);
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
        if (Vector3.Distance(transform.position, listStages[currentStage].position) > 2f)
        {
            SetTarget(listStages[currentStage].position);
        }
        Collider[] colliders = new Collider[5];
        int numCollider = Physics.OverlapSphereNonAlloc(transform.position, 2f, colliders, brickLayerMask);
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
        string s = "";
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
                    s = bridgeChild.name;
                    isFoundBridge = true;
                }
            }
            Debug.Log(s);
        }
        Debug.Log(s + "2");
        SetTarget(targetBridge);
    }

    public void FindCharacter()
    {
        Collider[] colliders = new Collider[5];
        int numCollider = Physics.OverlapSphereNonAlloc(transform.position, 3f, colliders, characterLayerMask);
        if(numCollider <= 0)
        {
            ChangeState(new PickBrickState());
            return;
        }
        float minDis = float.MaxValue;
        Vector3 newTarget = new Vector3();
        for (int i = 0; i < numCollider; i++)
        {
            if (!colliders[i].gameObject.name.Equals( this.gameObject.name))
            {
                Debug.Log("wtf");
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
        }
        SetTarget(newTarget);
    }
    public override void Fall()
    {
        base.Fall();
        
        ChangeState(new FallState());
    }
    
    public IEnumerator WakeUp() {
        //Play anim
        yield return new WaitForSeconds(1f);
        ChangeState(new PickBrickState());

    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.CompareTag("Bot"))
        {
            Bot bot = collision.gameObject.GetComponent<Bot>();
            if (bot.GetStackCount() > this.GetStackCount())
            {
                this.Fall();
                bot.ChangeState(new PickBrickState());
            }
            else if (bot.GetStackCount() < this.GetStackCount())
            {
                bot.Fall();
                this.ChangeState(new PickBrickState());
            }
            else
            {
                bot.Fall();
                this.Fall();
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player.GetStackCount() > this.GetStackCount())
            {
                this.Fall();
            }
            else if (player.GetStackCount() < this.GetStackCount())
            {
                player.Fall();
                this.ChangeState( new PickBrickState());
            }
            else
            {
                player.Fall();
                this.Fall();
            }
        }
    }

}

