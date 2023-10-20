using System;
using System.Collections;
using UnityEngine;
using UnityHFSM;

namespace Assets.Scripts.AIController.HFSM.States
{
    public class AttackState : EnemyStateBase
    {
        public AttackState(
            bool needsExitTime,
            Enemy Enemy,
            Action<State<EnemyState, StateEvent>> onEnter,
            float ExitTime = 4f):base(needsExitTime, Enemy, ExitTime, onEnter)
            {}

        public override void OnEnter()
        {
            Agent.isStopped = true;
            base.OnEnter();
            Animator.Play("AttackState");
        }
    }
}