using Godot;

namespace Game.Enemy
{
    public class ChaseState : EnemyStateBase
    {
        private readonly Node3D _target;

        public ChaseState(
            EnemyController controller,
            EnemyBase enemy,
            Node3D target)
            : base(controller, enemy)
        {
            _target = target;
        }

        public override void Enter()
        {
            GD.Print($"{Enemy.Name}: Enter Chase");
        }

        public override void PhysicsUpdate(double delta)
        {
            if (_target == null)
            {
                Enemy.Movement.Stop();
                Controller.ChangeState(new IdleState(Controller, Enemy));
                return;
            }

            Vector3 direction = _target.GlobalPosition - Enemy.GlobalPosition;
            direction.Y = 0f;

            //GD.Print($"Enemy: {Enemy.GlobalPosition}, Target: {_target.GlobalPosition}");
            Enemy.Movement.Move(direction);
        }

        public override void Exit()
        {
            Enemy.Movement.Stop();
        }
    }
}