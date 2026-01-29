using Godot;
using System;

public partial class DeathScreen : Control
{

    [Export] public Button RestartButton;
    [Export] public Button MainMenuButton;

    public override void _Ready()
    {
        if (RestartButton != null)
            RestartButton.Pressed += OnRestartButtonPressed;

        if (MainMenuButton != null)
            MainMenuButton.Pressed += OnMainMenuButtonPressed;
    }

    public void OnRestartButtonPressed()
    {
        if (GameManager.Instance != null)
        {
            var freshStats = ResourceLoader.Load<CharacterStats>("res://Resources/WarriorStats.tres");
            
            GameManager.Instance.CurrentStats = (CharacterStats)freshStats.Duplicate();
            
            GameManager.Instance.CurrentStats.UnspentPoints = 5;
        }

        GetTree().ChangeSceneToFile("res://Scenes/gui/StatSelection.tscn");
    }

    public void OnMainMenuButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/gui/MainMenu.tscn");
    }
}
