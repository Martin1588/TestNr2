using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AIController.HFSM.States
{
    public class CrawlState : EnemyStateBase        
    {
        private Player Target;

        public float speed = .5f;

        public CrawlState(bool needsExitTime, Enemy Enemy, Player Target) : base(needsExitTime, Enemy) {
            this.Target = Target;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Agent.isStopped = false;
            Agent.enabled = true;
            Animator.Play("CrawlState");
        }

        public override void OnLogic()
        {
            base.OnLogic();

            if (!RequestedExit)
            {
                //Enemy.Movement.GoTowardsPlayer(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>());
                Enemy.Movement.GoTowardsPlayer(Target, speed);
            }
            else if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                fsm.StateCanExit();
            }
        }
    }
}
