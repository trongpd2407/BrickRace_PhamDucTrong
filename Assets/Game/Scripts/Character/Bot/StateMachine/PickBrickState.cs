using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickBrickState : IState<Bot>
{
    public void OnEnter(Bot t)
    {
    }

    public void OnExecute(Bot t)
    {
        if (!t.GetIsFall())
        {
            t.FindBrick();
            t.Move();
            if (t.GetStackCount() > t.GetBuildBridgeThreshold())
            {
                t.ChangeState(new BuildBridgeState());
            }
            if (t.GetStackCount() >= t.GetAttackThreshold() && t.GetStackCount() <= t.GetAttackThreshold() + 2)
            {
                t.ChangeState(new AttackState());
            }
        }
     
    }

    public void OnExit(Bot t)
    {
    }
}
