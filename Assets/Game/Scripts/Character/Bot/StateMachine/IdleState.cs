using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState<Bot>
{
    public void OnEnter(Bot t)
    {
    }

    public void OnExecute(Bot t)
    {
        t.StartCoroutine(WaitSecond(t));
    }

    public void OnExit(Bot t)
    {
    }
    public IEnumerator WaitSecond(Bot t)
    {
        yield return new WaitForSeconds(0.5f);
        t.ChangeState(new PickBrickState());
    }
}
