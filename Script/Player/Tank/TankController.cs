using Godot;

public partial class TankController : VehicleBody3D
{
    [ExportGroup("References")]
    [Export] public Node3D CameraPivot;
    [Export] public Node3D TurretPivot;
    [Export] public Node3D BarrelPivot;

    [ExportGroup("Movement")]
    [Export] public float EngineForce = 3000f;
    [Export] public float TurnForce = 2500f;
    [Export] public float BrakeForce = 80f;

    [ExportGroup("Turning")]
    [Export] public bool UsePivotTorque = true;
    [Export] public float PivotTorque = 6000f;

    [ExportGroup("Wheels")]
    [Export] public Godot.Collections.Array<VehicleWheel3D> LeftWheels = new();
    [Export] public Godot.Collections.Array<VehicleWheel3D> RightWheels = new();

    public override void _PhysicsProcess(double delta)
    {
        HandleTankMovement();
        UpdateTurretFacing();
        UpdateBarrelFacing();
    }

    private void HandleTankMovement()
    {
        Vector2 input = PlayerInput.Movement;

        float throttle = input.Y; // W/S
        float turn = input.X;     // A/D

        if (Mathf.IsZeroApprox(throttle) && Mathf.IsZeroApprox(turn))
        {
            ApplyIdleBrakes();
            return;
        }

        ClearBrakes();

        float leftForce = throttle * EngineForce + turn * TurnForce;
        float rightForce = throttle * EngineForce - turn * TurnForce;

        ApplyWheelForces(LeftWheels, leftForce);
        ApplyWheelForces(RightWheels, rightForce);

        if (UsePivotTorque && !Mathf.IsZeroApprox(turn))
        {
            ApplyTorque(GlobalBasis.Y * -turn * PivotTorque);
        }
    }

    private void ClearBrakes()
    {
        ApplyBrakes(LeftWheels, 0f);
        ApplyBrakes(RightWheels, 0f);
    }

    private void ApplyIdleBrakes()
    {
        ApplyWheelForces(LeftWheels, 0f);
        ApplyWheelForces(RightWheels, 0f);

        ApplyBrakes(LeftWheels, BrakeForce);
        ApplyBrakes(RightWheels, BrakeForce);
    }

    private void ApplyWheelForces(
        Godot.Collections.Array<VehicleWheel3D> wheels,
        float force)
    {
        foreach (VehicleWheel3D wheel in wheels)
        {
            if (wheel == null)
                continue;

            wheel.EngineForce = force;
        }
    }

    private void ApplyBrakes(
        Godot.Collections.Array<VehicleWheel3D> wheels,
        float brake)
    {
        foreach (VehicleWheel3D wheel in wheels)
        {
            if (wheel == null)
                continue;

            wheel.Brake = brake;
        }
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

        localForward.Y = 0f;

        if (localForward.LengthSquared() < 0.001f)
            return;

        localForward = localForward.Normalized();

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

        if (localForward.LengthSquared() < 0.001f)
            return;

        localForward = localForward.Normalized();

        float localPitch = Mathf.Atan2(localForward.Y, -localForward.Z);

        Vector3 rotation = BarrelPivot.Rotation;
        rotation.X = localPitch;
        BarrelPivot.Rotation = rotation;
    }
}