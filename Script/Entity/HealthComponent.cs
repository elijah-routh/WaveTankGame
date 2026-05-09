using Godot;
using Game.Entity;

namespace Game.Components
{
    public partial class HealthComponent : Node, IDamageable, IHealable, IKillable
    {
        [Signal] public delegate void HealthChangedEventHandler(float currentHealth, float maxHealth);
        [Signal] public delegate void DamagedEventHandler(float damage);
        [Signal] public delegate void HealedEventHandler(float amount);
        [Signal] public delegate void DiedEventHandler();

        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }
        [Export] public bool DestroyOwnerOnDeath { get; private set; } = false;
        public bool IsDead { get; private set; }

        public override void _Ready()
        {
            CurrentHealth = MaxHealth;
            EmitSignal(SignalName.HealthChanged, CurrentHealth, MaxHealth);
        }

        public void Initialize(float maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = MaxHealth;
            IsDead = false;

            EmitSignal(SignalName.HealthChanged, CurrentHealth, MaxHealth);
        }

        public void TakeDamage(float damage)
        {
            GD.Print($"HealthComp take damage");

            if (IsDead || damage <= 0f)
                return;

            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0f);
            GD.Print(CurrentHealth);

            EmitSignal(SignalName.Damaged, damage);
            EmitSignal(SignalName.HealthChanged, CurrentHealth, MaxHealth);

            if (CurrentHealth <= 0f)
                Kill();
        }

        public void Heal(float amount)
        {
            if (IsDead || amount <= 0f)
                return;

            float previousHealth = CurrentHealth;

            CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);

            float healedAmount = CurrentHealth - previousHealth;

            if (healedAmount <= 0f)
                return;

            EmitSignal(SignalName.Healed, healedAmount);
            EmitSignal(SignalName.HealthChanged, CurrentHealth, MaxHealth);
        }

        public void Kill()
        {
            if (IsDead)
                return;

            IsDead = true;
            CurrentHealth = 0f;

            EmitSignal(SignalName.HealthChanged, CurrentHealth, MaxHealth);
            EmitSignal(SignalName.Died);

            if (DestroyOwnerOnDeath)
                Owner?.QueueFree();
        }

        public void ResetHealth()
        {
            IsDead = false;
            CurrentHealth = MaxHealth;

            EmitSignal(SignalName.HealthChanged, CurrentHealth, MaxHealth);
        }
    }
}