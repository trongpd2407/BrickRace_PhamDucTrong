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
        t.PickBrick();
        t.Move();
    }

    public void OnExit(Bot t)
    {
    }
}
