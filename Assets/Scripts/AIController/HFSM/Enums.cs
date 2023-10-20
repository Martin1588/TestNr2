using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.AIController.HFSM
{
    public enum EnemyState
    {
        Idle,
        Walk,
        Crawl,
        Attack,
        MeleeAttack,
        Die
    }

    public enum StateEvent
    {
        DetectPlayer,
        LostPlayer,
        AttackPlayer,
        NoHealth,
        LowHealth
    }
}