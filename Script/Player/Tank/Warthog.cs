using Godot;

public partial class Warthog : VehicleBody3D
{
    [ExportGroup("Wheels")]
    [Export] public VehicleWheel3D FrontLeftWheel;
    [Export] public VehicleWheel3D RearLeftWheel;
    [Export] public VehicleWheel3D FrontRightWheel;
    [Export] public VehicleWheel3D RearRightWheel;

    [ExportGroup("Steering")]
    [Export] public float MaxSteerAngle = 0.45f;
    [Export] public float SteerSpeed = 6f;

    [ExportGroup("Suspension")]
    [Export] public float WheelFriction = 10.5f;
    [Export] public float SuspensionStiffness = 0.0f;

    [ExportGroup("Engine")]
    [Export] public float Acceleration = 1200f;
    [Export] public float MaxSpeed = 30f;

    private float _throttle;

    private VehicleWheel3D[] _wheels;

    private float _steerInput;

    public override void _Ready()
    {
        _wheels =
        [
            FrontLeftWheel,
            RearLeftWheel,
            FrontRightWheel,
            RearRightWheel
        ];

        ApplyWheelSettings();
    }

    public override void _PhysicsProcess(double delta)
    {
        _throttle = Input.GetAxis("move_back", "move_forward");

        HandleEngineVelocity();

        _steerInput = Input.GetAxis("move_right", "move_left");

        HandleSteering((float)delta);
    }

    private void ApplyWheelSettings()
    {
        foreach (VehicleWheel3D wheel in _wheels)
        {
            if (wheel == null)
                continue;

            wheel.WheelFrictionSlip = WheelFriction;
            wheel.SuspensionStiffness = SuspensionStiffness;
        }
    }

    private void HandleEngineVelocity()
    {
        float speed = LinearVelocity.Length();

        float speedFactor =
            1.0f - Mathf.Min(speed / MaxSpeed, 1.0f);

        Vector3 forward =
            -GlobalTransform.Basis.Z;

        Vector3 force =
            forward * Acceleration * _throttle * speedFactor;

        ApplyCentralForce(force);
    }

    private void HandleSteering(float delta)
    {
        float targetSteer = _steerInput * MaxSteerAngle;

        FrontLeftWheel.Steering = Mathf.MoveToward(
            FrontLeftWheel.Steering,
            targetSteer,
            SteerSpeed * delta
        );

        FrontRightWheel.Steering = Mathf.MoveToward(
            FrontRightWheel.Steering,
            targetSteer,
            SteerSpeed * delta
        );
    }

    
}
