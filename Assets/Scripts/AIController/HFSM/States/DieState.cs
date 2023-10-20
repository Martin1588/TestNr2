using System;
using System.Collections;
using UnityEngine;
using UnityHFSM;

namespace Assets.Scripts.AIController.HFSM.States
{
    public class DieState : EnemyStateBase
    {

        public DieState(
            bool needsExitTime,
            Enemy Enemy,
            Action<State<EnemyState, StateEvent>> onEnter,
            float ExitTime = .35f) : base(needsExitTime, Enemy, ExitTime, onEnter)
        { }

        public override void OnEnter()
        {
            Agent.isStopped = true;
            base.OnEnter();
            Animator.Play("DieState");
        }
    }
}