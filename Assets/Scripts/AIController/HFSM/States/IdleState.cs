using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AIController.HFSM.States
{
    public class IdleState : EnemyStateBase
    {
        public IdleState(bool needsExitTime, Enemy Enemy) : base(needsExitTime, Enemy) { }


        public override void OnEnter()
        {
            base.OnEnter();
            Agent.isStopped = true;
            Animator.Play("IdleState");
        }
    }
}