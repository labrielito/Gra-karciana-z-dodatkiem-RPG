using Godot;
using System;

[Tool]
public partial class EnemySpawner : Node2D
{
    [ExportGroup("Co spawnować?")]
    [Export] public PackedScene EnemyScene; 

    [ExportGroup("Gdzie i ile?")]
    [Export] public int SpawnCount = 3;     
    [Export] public Vector2 SpawnArea = new Vector2(200, 200); 

    [ExportGroup("Poziomy Trudności")]
    [Export] public int MinLevel = 1;
    [Export] public int MaxLevel = 3;

    public override void _Ready()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (EnemyScene == null)
        {
            GD.PrintErr("Spawner: Nie podałeś sceny przeciwnika!");
            return;
        }

        var rng = new RandomNumberGenerator();
        rng.Randomize();

        for (int i = 0; i < SpawnCount; i++)
        {
            SkeletonWarriorEnemy newEnemy = EnemyScene.Instantiate<SkeletonWarriorEnemy>();

            newEnemy.Level = rng.RandiRange(MinLevel, MaxLevel);

            float randomX = rng.RandfRange(0, SpawnArea.X);
            float randomY = rng.RandfRange(0, SpawnArea.Y);
            
            newEnemy.Position = new Vector2(randomX, randomY);

            AddChild(newEnemy);
        }
    }

    public override void _Draw()
    {
        if (Engine.IsEditorHint()) 
        {
            DrawRect(new Rect2(Vector2.Zero, SpawnArea), new Color(1, 0, 0, 0.3f)); 
        }
    }

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
        {
            QueueRedraw();
        }
    }
}
