using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUpState : IState<Bot>
{
    public void OnEnter(Bot bot)
    {
    }

    public void OnExecute(Bot bot)
    {
        bot.StartCoroutine(bot.WakeUp());
        bot.ChangeState(new PickBrickState());
    }

    public void OnExit(Bot bot)
    {
    }
}
