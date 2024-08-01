using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState<Bot>
{
    public void OnEnter(Bot t)
    {
    }

    public void OnExecute(Bot t)
    {
        t.FindCharacter();
        t.Move();
    }

    public void OnExit(Bot t)
    {
    }
}
