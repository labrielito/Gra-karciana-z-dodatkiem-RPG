using Godot;
using System;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; } 

    public CharacterStats CurrentStats;
    public CharacterStats PlayerStatsPending;
    public CharacterStats EnemyStatsPending;

    public override void _Ready()
    {
        Instance = this;

        var loadedStats = ResourceLoader.Load<CharacterStats>("res://Resources/WarriorStats.tres");
        CurrentStats = (CharacterStats)loadedStats.Duplicate();

        CurrentStats.UnspentPoints = 5;

    }


}
