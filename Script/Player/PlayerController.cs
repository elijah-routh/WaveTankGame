using Godot;

public partial class PlayerController : CharacterBody3D
{
    [Export] public Node3D CameraPivot;
    //[Export] public Node3D Body;

    private PlayerMovement _movement =
        new PlayerMovement();

    public override void _PhysicsProcess(double delta)
    {
        float dt = (float)delta;

        _movement.ApplyGravity(this, dt);

        _movement.HandleJump(this);

        _movement.HandleMovement(
            this,
            CameraPivot,
            dt
        );

        MoveAndSlide();
    }
}