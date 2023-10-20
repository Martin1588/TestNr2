using Assets.Scripts.AIController.HFSM.States;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;
using Assets.Scripts;
using Assets.Scripts.Target;

namespace Assets.Scripts.AIController.HFSM
{
    public class Enemy : MonoBehaviour
    {
        private StateMachine<EnemyState, StateEvent> EnemyFSM;
        private Animator animator;
        private NavMeshAgent agent;

        public EnemyMovement Movement;
        public TargetBehaviour HealthSensor;

        [SerializeField]
        private Player Player;

        private Player Target;

        //Spit

        [Header("Sensors")]
        [SerializeField]
        private PlayerSensor ChasePlayerSensor;
        [SerializeField]
        private PlayerSensor MeleePlayerSensor;

        private bool IsInChasingRange;
        private bool IsInMeleeRange;
        private bool HasLostAllHealth;
        private bool HasLowHealth;

        [Space]
        [Header("Debug Info")]
        [SerializeField]
        private bool IsInSpitRange;
        [SerializeField]
        private float LastAttackTime;
        [SerializeField]
        private float LastBounceTime;
        [SerializeField]
        private float LastRollTime;

        [Header("Attack Config")]
        [SerializeField]
        [Range(0.1f, 5f)]
        private float AttackCooldown = 2;

        // Use this for initialization
        private void Awake()
        {            
            Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            Movement = GetComponent<EnemyMovement>();
            HealthSensor = GetComponent<TargetBehaviour>();

            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            EnemyFSM = new StateMachine<EnemyState, StateEvent>();

            EnemyFSM.AddState(name: EnemyState.Idle, new IdleState(false, this));
            EnemyFSM.AddState(name: EnemyState.Walk, new WalkState(false, this, Target));
            EnemyFSM.AddState(name: EnemyState.Crawl, new CrawlState(false, this, Target));
            //EnemyFSM.AddState(name: EnemyState.Attack, new AttackState(true, this, OnAttack));
            EnemyFSM.AddState(name: EnemyState.Attack, new MeleeAttackState(true, this, OnAttack));
            EnemyFSM.AddState(name: EnemyState.Die, new DieState(false, this, OnDeath));

            EnemyFSM.AddTriggerTransition(StateEvent.DetectPlayer, new Transition<EnemyState>(EnemyState.Idle, EnemyState.Walk));
            EnemyFSM.AddTriggerTransition(StateEvent.LostPlayer, new Transition<EnemyState>(EnemyState.Walk, EnemyState.Idle));

            EnemyFSM.AddTriggerTransition(StateEvent.NoHealth, new Transition<EnemyState>(EnemyState.Idle, EnemyState.Die));
            EnemyFSM.AddTriggerTransition(StateEvent.NoHealth, new Transition<EnemyState>(EnemyState.Walk, EnemyState.Die));
            EnemyFSM.AddTriggerTransition(StateEvent.NoHealth, new Transition<EnemyState>(EnemyState.Attack, EnemyState.Die));
            EnemyFSM.AddTriggerTransition(StateEvent.NoHealth, new Transition<EnemyState>(EnemyState.Crawl, EnemyState.Die));

            EnemyFSM.AddTriggerTransition(StateEvent.LowHealth, new Transition<EnemyState>(EnemyState.Walk, EnemyState.Crawl));
            EnemyFSM.AddTriggerTransition(StateEvent.LowHealth, new Transition<EnemyState>(EnemyState.Attack, EnemyState.Crawl));
            EnemyFSM.AddTriggerTransition(StateEvent.LowHealth, new Transition<EnemyState>(EnemyState.Idle, EnemyState.Crawl));


            EnemyFSM.AddTransition(
                new Transition<EnemyState>(EnemyState.Idle, EnemyState.Walk, (transition) => IsInChasingRange 
                && Vector3.Distance(Player.transform.position, transform.position) > agent.stoppingDistance)
            );
            EnemyFSM.AddTransition(
                new Transition<EnemyState>(EnemyState.Walk, EnemyState.Idle, (transition) => !IsInChasingRange 
                || Vector3.Distance(Player.transform.position, transform.position) <= agent.stoppingDistance)
            );
            EnemyFSM.AddTransition(
                new Transition<EnemyState>(EnemyState.Idle, EnemyState.Die, (transition) => HasLostAllHealth, null, null, true)
            );
            EnemyFSM.AddTransition(
                new Transition<EnemyState>(EnemyState.Walk, EnemyState.Die, (transition) => HasLostAllHealth, null, null, true)
            );
            EnemyFSM.AddTransition(
                new Transition<EnemyState>(EnemyState.Attack, EnemyState.Die, (transition) => HasLostAllHealth, null, null, true)
            );
            EnemyFSM.AddTransition(
                new Transition<EnemyState>(EnemyState.Crawl, EnemyState.Die, (transition) => HasLostAllHealth, null, null, true)
            );
            EnemyFSM.AddTransition(
               new Transition<EnemyState>(EnemyState.Idle, EnemyState.Crawl, (transition) => HasLowHealth, null, null, true)
           );
            EnemyFSM.AddTransition(
                new Transition<EnemyState>(EnemyState.Walk, EnemyState.Crawl, (transition) => HasLowHealth, null, null, true)
            );
            EnemyFSM.AddTransition(
                new Transition<EnemyState>(EnemyState.Attack, EnemyState.Crawl, (transition) => HasLowHealth, null, null, true)
            );

            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Walk, EnemyState.Attack, ShouldMelee, null, null, true));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Attack, ShouldMelee, null, null, true));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Walk, IsNotWithinIdleRange));
            EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Idle, IsWithinIdleRange));

            EnemyFSM.Init();
        }

        private void Start()
        {
            ChasePlayerSensor.OnPlayerEnter += ChasePlayerSensor_OnPlayerEnter;
            ChasePlayerSensor.OnPlayerExit += ChasePlayerSensor_OnPlayerExit;
            MeleePlayerSensor.OnPlayerEnter += MeleePlayerSensor_OnPlayerEnter;
            MeleePlayerSensor.OnPlayerExit += MeleePlayerSensor_OnPlayerExit;

            HealthSensor.LostAllHealth += Target_OnLostAllHealth;
            HealthSensor.LowHealth += Target_OnLowHealth;
        }

        private void Target_OnLostAllHealth()
        {
            EnemyFSM.Trigger(StateEvent.NoHealth);
            HasLowHealth = false;
            HasLostAllHealth = true;
        }

        private void Target_OnLowHealth()
        {
            EnemyFSM.Trigger(StateEvent.LowHealth);
            HasLowHealth = true;
        }

        private void ChasePlayerSensor_OnPlayerEnter(Transform Player)
        {
            EnemyFSM.Trigger(StateEvent.DetectPlayer);
            IsInChasingRange = true;
        }

        private void ChasePlayerSensor_OnPlayerExit(Vector3 LastKnownPosition)
        {
            EnemyFSM.Trigger(StateEvent.LostPlayer);
            IsInChasingRange = false;
        }
        private void MeleePlayerSensor_OnPlayerEnter(Transform Player) => IsInMeleeRange = true;

        private void MeleePlayerSensor_OnPlayerExit(Vector3 LastKnownPosition) => IsInMeleeRange = false;

        private bool ShouldMelee(Transition<EnemyState> Transition) =>
            LastAttackTime + AttackCooldown <= Time.time
           && IsInMeleeRange;

        private bool IsWithinIdleRange(Transition<EnemyState> Transition) =>
            agent.remainingDistance <= agent.stoppingDistance;

        private bool IsNotWithinIdleRange(Transition<EnemyState> Transition) =>
            !IsWithinIdleRange(Transition);

        private bool ShouldDie(Transition<EnemyState> Transition) =>
            HealthSensor.health <= 0;

        private void OnAttack(State<EnemyState,StateEvent> state) { }
        private void OnDeath(State<EnemyState, StateEvent> state) { }

        //private void OnBounce(State<EnemyState, StateEvent> state) { }
        //private void OnRoll(State<EnemyState, StateEvent> state) { }


        // Update is called once per frame
        void Update()
        {
            EnemyFSM.OnLogic();
        }
    }
}