using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : IState<Bot>
{
    public void OnEnter(Bot t)
    {
    }

    public void OnExecute(Bot t)
    {
        t.StartCoroutine(t.TurnOffCollider());
        t.ChangeState(new WakeUpState());
    }

    public void OnExit(Bot t)
    {
    }
}
