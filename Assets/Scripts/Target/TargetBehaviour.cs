using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Target
{
    public class TargetBehaviour : MonoBehaviour
    {
        public float health = 100f;

        public delegate void SelfStatus();
        public delegate void TargetHealthStatus();

        public event TargetHealthStatus LostAllHealth;
        public event TargetHealthStatus LowHealth;

        public void TakeDamage(float amount) {
            Debug.Log(amount + " Damage Taken");

            health -= amount;
            HealthChange();
        }

        async void HealthChange()
        {
            if (health <= 0f) OnLostAllHealth();
            if (health <= 10f) OnLowHealth();
        }

        void OnLostAllHealth()
        {
            LostAllHealth.Invoke();
        }
        void OnLowHealth()
        {
            LowHealth.Invoke();
        }
    }
}