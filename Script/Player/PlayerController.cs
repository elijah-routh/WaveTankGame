using Godot;

public partial class PlayerController : CharacterBody3D
{
    [Export] public Node3D CameraPivot;
    [Export] public Node3D TurretPivot;
    [Export] public Node3D BarrelPivot;


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
        UpdateBarrelFacing();
    }

    private void UpdateTurretFacing()
    {
        if (TurretPivot == null || CameraPivot == null)
            return;

        Vector3 turretRotation = TurretPivot.GlobalRotation;
        turretRotation.Y = CameraPivot.GlobalRotation.Y;
        TurretPivot.GlobalRotation = turretRotation;
    }

    private void UpdateBarrelFacing()
    {
        if (BarrelPivot == null || CameraPivot == null)
            return;

        Vector3 barrelRotation = BarrelPivot.GlobalRotation;
        barrelRotation.X = CameraPivot.GlobalRotation.X;
        BarrelPivot.GlobalRotation = barrelRotation;
    }
}