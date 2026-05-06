using Godot;

public static class PlayerInput
{
    public static Vector2 Movement =>
        Input.GetVector(
            "move_left",
            "move_right",
            "move_forward",
            "move_back"
        );

    public static bool JumpPressed =>
        Input.IsActionJustPressed("jump");
}
