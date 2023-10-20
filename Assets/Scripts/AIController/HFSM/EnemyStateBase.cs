using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Assets.Scripts.AIController.HFSM
{
    public class EnemyStateBase : State<EnemyState,StateEvent>
    {
        protected readonly Enemy Enemy;
        protected readonly NavMeshAgent Agent;
        protected readonly Animator Animator;

        protected bool RequestedExit;
        protected float ExitTime;

        protected readonly Action<State<EnemyState, StateEvent>> onEnter;
        protected readonly Action<State<EnemyState, StateEvent>> onExit;
        protected readonly Action<State<EnemyState, StateEvent>> onLogic;
        protected readonly Func<State<EnemyState, StateEvent>, bool> canExit;

        public EnemyStateBase(
            bool needsExitTime,
            Enemy Enemy,
            float Exittime = .1f,
            Action<State<EnemyState, StateEvent>> onEnter = null,
            Action<State<EnemyState, StateEvent>> onExit = null,
            Action<State<EnemyState, StateEvent>> onLogic = null,
            Func<State<EnemyState, StateEvent>, bool> canExit = null
            )
        {
            this.Enemy = Enemy;
            this.onEnter = onEnter;
            this.onExit = onExit;
            this.onLogic = onLogic;
            this.canExit = canExit;
            this.ExitTime = Exittime;   
            this.needsExitTime = needsExitTime;
            Agent = Enemy.GetComponent<NavMeshAgent>();
            Animator = Enemy.GetComponent<Animator>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            RequestedExit = false;
            onEnter?.Invoke( this );
        }

        public override void OnLogic()
        {
            base.OnLogic();
            if(RequestedExit && timer.Elapsed >= ExitTime)
            {
                fsm.StateCanExit();
            }
        }

        public override void OnExitRequest()
        {
            if (!needsExitTime || canExit != null && canExit(this))
            {
                fsm.StateCanExit();
            }
            else
            {
                RequestedExit = true;
            }
        }
    }
}