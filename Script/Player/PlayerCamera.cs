using Godot;

public partial class PlayerCamera : Node3D
{

    [Export] public Node3D Target;

    [Export] public float MouseSensitivity = 0.002f;
    [Export] public float MinPitch = -60f;
    [Export] public float MaxPitch = 45f;
    [Export] public Vector3 Offset = new Vector3(0, 1.5f, 0);

    private Vector2 _cameraInput = Vector2.Zero;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        bool isCameraMotion =
            @event is InputEventMouseMotion &&
            Input.MouseMode == Input.MouseModeEnum.Captured;

        if (!isCameraMotion)
            return;

        InputEventMouseMotion mouse =
            @event as InputEventMouseMotion;

        _cameraInput =
            mouse.ScreenRelative * MouseSensitivity;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Target != null)
        {
            GlobalPosition = Target.GlobalPosition + Offset;
        }

        Rotation += new Vector3(
            -_cameraInput.Y,
            -_cameraInput.X,
            0
        );

        Rotation = new Vector3(
            Mathf.Clamp(
                Rotation.X,
                Mathf.DegToRad(MinPitch),
                Mathf.DegToRad(MaxPitch)
            ),
            Rotation.Y,
            0
        );

        _cameraInput = Vector2.Zero;
    }
}
