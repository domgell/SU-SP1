using System;
using UnityEngine;

namespace Game
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float damageCooldown = 0.1f;

        public float TimeSinceDamage { get; private set; }
        public float CurrentHealth { get; private set; }
        public bool IsAlive => CurrentHealth > 0;
        public float DamageCooldown => damageCooldown;
        public float MaxHealth => maxHealth;
        public event Action<float> OnDamage;
        public event Action OnDeath;

        private void Awake()
        {
            CurrentHealth = maxHealth;
            TimeSinceDamage = damageCooldown;
        }

        /// <summary>
        /// Tries to damage the entity. Returns true if damage was applied, false if still in cooldown.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool TryDamage(float amount)
        {
            if (damageCooldown > TimeSinceDamage)
                return false;

            TimeSinceDamage = 0;
            CurrentHealth = MathF.Max(0, CurrentHealth - amount);

            if (CurrentHealth == 0) OnDeath?.Invoke();
            else OnDamage?.Invoke(amount);

            return true;
        }

        public void Kill()
        {
            if (!IsAlive) return;
        
            CurrentHealth = 0;
            OnDeath?.Invoke();
        }

        public void Reset()
        {
            CurrentHealth = maxHealth;
            TimeSinceDamage = damageCooldown;
        }

        private void Update()
        {
            TimeSinceDamage += Time.deltaTime;
        }
    }
}