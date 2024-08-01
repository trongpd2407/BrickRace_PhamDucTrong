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
        t.Fall();
    }

    public void OnExit(Bot t)
    {
    }
}
