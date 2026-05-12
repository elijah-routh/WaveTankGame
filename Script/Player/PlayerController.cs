using Godot;

public partial class PlayerController : CharacterBody3D
{
    [Export] public Node3D CameraPivot;
    [Export] public Node3D TurretPivot;
    [Export] public Node3D BarrelPivot;
    [Export] public PlayerMoveComponent Movement;
    [Export] public GroundAlignComponent GroundAlignment;

    public override void _PhysicsProcess(double delta)
    {
        float dt = (float)delta;

        Movement.ApplyGravity(this, dt);
        Movement.HandleJump(this);
        Movement.HandleMovement(this, CameraPivot, dt);

        MoveAndSlide();

        GroundAlignment.AlignToGround(this, dt);

        UpdateTurretFacing();
        UpdateBarrelFacing();
    }

    private void UpdateTurretFacing()
    {
        if (TurretPivot == null || CameraPivot == null)
            return;

        Node3D parent = TurretPivot.GetParent<Node3D>();
        if (parent == null)
            return;

        Vector3 cameraForward = -CameraPivot.GlobalBasis.Z;

        Vector3 localForward = parent.GlobalBasis.Inverse() * cameraForward;

        float localYaw = Mathf.Atan2(localForward.X, localForward.Z);

        Vector3 rotation = TurretPivot.Rotation;
        rotation.Y = localYaw;
        TurretPivot.Rotation = rotation;
    }

    private void UpdateBarrelFacing()
    {
        if (BarrelPivot == null || CameraPivot == null)
            return;

        Node3D parent = BarrelPivot.GetParent<Node3D>();
        if (parent == null)
            return;

        Vector3 cameraForward = -CameraPivot.GlobalBasis.Z;

        Vector3 localForward = parent.GlobalBasis.Inverse() * cameraForward;

        float localPitch = Mathf.Atan2(localForward.Y, -localForward.Z);

        Vector3 rotation = BarrelPivot.Rotation;
        rotation.X = localPitch;
        BarrelPivot.Rotation = rotation;
    }
}