using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBridgeState : IState<Bot>
{
    public void OnEnter(Bot t)
    {
        t.SetNextStage();
        Transform nextStage = t.GetNextStage();
        t.FindBridge(nextStage);
    }
    public void OnExecute(Bot t)
    {
        Vector3 target = t.GetTarget();
        Vector3 targetBridge = t.GetTargetBridge();
        if(target == targetBridge && Vector3.Distance(t.transform.position,targetBridge) < 0.1f)
        {
            if (t.GetStackCount() > 0)
            {
                t.SetTarget(t.GetNextStage().position);
            }
            else
            {
                t.ChangeState(new PickBrickState());
            }
        }
        if (!t.GetIsFall())
        {
            t.Move();
        }
       
    }
    public void OnExit(Bot t)
    {
    }
}
