using Godot;

public partial class PlayerController : CharacterBody3D
{
    [Export] public Node3D CameraPivot;
    [Export] public Node3D TurretPivot;

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

        UpdateTurretFacing();
    }

    private void UpdateTurretFacing()
    {
        if (TurretPivot == null || CameraPivot == null)
            return;

        Vector3 turretRotation = TurretPivot.GlobalRotation;
        turretRotation.Y = CameraPivot.GlobalRotation.Y;
        TurretPivot.GlobalRotation = turretRotation;
    }
}