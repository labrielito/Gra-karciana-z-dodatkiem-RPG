using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] public float MaxSpeed = 300.0f;
    [Export] public float Acceleration = 1800.0f;
    [Export] public float Friction = 2000.0f;

    public CharacterStats Stats;

    [ExportGroup("Visuals")]
    [Export] public Texture2D TextureFront;
    [Export] public Texture2D TextureBack;

    private Sprite2D _sprite;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");
        
        if (GameManager.Instance != null)
        {
            Stats = GameManager.Instance.CurrentStats;

            if (Stats != null)
            {
                MaxSpeed = 150 + (Stats.Speed * 15);
            }
        }
        else
        {
            GD.PrintErr("BŁĄD: Player nie widzi GameManagera!");
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        float fDelta = (float)delta;
        Vector2 velocity = Velocity;

        Vector2 direction = Input.GetVector("Move_Left", "Move_Right", "Move_Up", "Move_Down");
        if (direction != Vector2.Zero)
        {
            velocity = velocity.MoveToward(direction * MaxSpeed, Acceleration * fDelta) ;
        }
        else
        {
            velocity = velocity.MoveToward(Vector2.Zero, Friction * fDelta);
        }

        Velocity = velocity;
        MoveAndSlide();

        UpdateLook(direction);
    }

    private void UpdateLook(Vector2 direction)
    {
        if (direction == Vector2.Zero) return;

        if (direction.Y < 0)
        {
            _sprite.Texture = TextureBack;
        }
        else if (direction.Y > 0)
        {
            _sprite.Texture = TextureFront;
        }

        if (direction.X != 0)
        {
            _sprite.FlipH = direction.X < 0;
        }
    }
}
