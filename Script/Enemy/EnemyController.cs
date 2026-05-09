using Godot;
using Game.Entity;

namespace Game.Enemy
{
    public partial class EnemyController : Node
    {
        private EnemyBase _enemy;
        private StateMachine _stateMachine;

        [Export] public Node3D Target { get; set; }

        public void Initialize(EnemyBase enemy)
        {
            _enemy = enemy;

            _stateMachine = new StateMachine();

            Target = GetTree().GetFirstNodeInGroup("player") as Node3D;

            GD.Print($"{Name}: Target found = {Target?.Name}");

            _enemy.Health.Damaged += OnDamaged;
            _enemy.Health.Died += OnDied;

            ChangeState(new IdleState(this, _enemy));
        }

        public override void _Process(double delta)
        {
            _stateMachine?.Update(delta);
        }

        public override void _PhysicsProcess(double delta)
        {
            _stateMachine?.PhysicsUpdate(delta);
        }

        public void ChangeState(EnemyStateBase state)
        {
            _stateMachine?.ChangeState(state);
        }

        private void OnDamaged(float damage)
        {
            GD.Print($"{Name}: Controller received damage signal: {damage}");

            if (_stateMachine.IsInState<ChaseState>())
                return;

            if (Target == null)
            {
                GD.Print($"{Name}: Target is null. Cannot chase.");
                return;
            }

            ChangeState(new ChaseState(this, _enemy, Target));
        }

        private void OnDied()
        {
            ChangeState(new DeadState(this, _enemy));
        }
    }
}