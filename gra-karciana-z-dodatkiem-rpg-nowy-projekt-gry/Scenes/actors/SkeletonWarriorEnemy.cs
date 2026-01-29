using Godot;
using System;

public partial class SkeletonWarriorEnemy : CharacterBody2D
{
    [ExportGroup("Ustawienia RPG")]
    [Export] public CharacterStats Stats; 
    [Export] public int Level = 1;        
    [Export] public int PointsPerLevel = 3; 

    [ExportGroup("Interfejs (UI)")]
    [Export] public Label LevelLabel;
    [Export] public Label HPLabel;

    public override void _Ready()
    {
        
        if (Stats == null)
        {
            GD.PrintErr("BŁĄD: Szkielet nie ma statystyk! Podepnij SkeletonStats.tres.");
            return;
        }
      
        Stats = (CharacterStats)Stats.Duplicate();

        GenerateRandomStats();

        UpdateUI();
    }

    private void GenerateRandomStats()
    {
        Stats.Level = Level;

        int pointsToDistribute = (Level - 1) * PointsPerLevel;

        var rng = new RandomNumberGenerator();
        rng.Randomize(); 


        for (int i = 0; i < pointsToDistribute; i++)
        {
            int choice = rng.RandiRange(0, 3);

            switch (choice)
            {
                case 0: Stats.Strength++; break;     
                case 1: Stats.Protection++; break;   
                case 2: Stats.Agility++; break;      
                case 3: Stats.Luck++; break;         
            }
        }

        Stats.CurrentHealth = Stats.GetMaxHealth()-40;
    }

    public void UpdateUI()
    {
        if (LevelLabel != null)
        {
            LevelLabel.Text = $"Lvl {Level}";
        }

        if (HPLabel != null)
        {
            HPLabel.Text = $"HP: {Stats.CurrentHealth}/{Stats.GetMaxHealth()-40}";
        }
    }

    private void _on_detection_zone_body_entered(Node2D body)
    {
        if (body.Name == "Player") 
        {
            GD.Print("Gracz wykryty! Rozpoczynam walkę...");
            StartBattle(body);
        }
    }

    private void StartBattle(Node2D playerBody)
    {
 
        var playerScript = playerBody as Player;
        
        if (playerScript == null || playerScript.Stats == null)
        {
            GD.PrintErr("Błąd: Nie mogę pobrać statystyk gracza!");
            return;
        }

        GameManager.Instance.PlayerStatsPending = playerScript.Stats;
        GameManager.Instance.EnemyStatsPending = this.Stats;

        GetTree().ChangeSceneToFile("res://Scenes/gui/BattleScene.tscn");
    }
}
