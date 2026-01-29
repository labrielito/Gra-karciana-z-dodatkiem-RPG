using Godot;
using System;
using System.Collections.Generic;

public partial class StatSelection : Control
{
    private CharacterStats _stats;
    private Dictionary<string, int> _baseStats = new Dictionary<string, int>();
    private Label _pointsLabel;
    private Button _startButton;

    public override void _Ready()
    {
        _stats = GetNode<GameManager>("/root/GameManager").CurrentStats;

        _baseStats["Strength"] = _stats.Strength;
        _baseStats["Agility"] = _stats.Agility;
        _baseStats["Intelligence"] = _stats.Intelligence;
        _baseStats["Protection"] = _stats.Protection;
        _baseStats["Speed"] = _stats.Speed;
        _baseStats["Luck"] = _stats.Luck;

        _pointsLabel = FindChild("PointsLabel", true, false) as Label;

        SetupStatRow("StrengthRow", "Strength");
        SetupStatRow("AgilityRow", "Agility");
        SetupStatRow("IntelligenceRow", "Intelligence");
        SetupStatRow("ProtectionRow", "Protection");
        SetupStatRow("SpeedRow", "Speed");
        SetupStatRow("LuckRow", "Luck");

        (FindChild("StartButton", true, false) as Button).Pressed += OnStartButtonPressed;

        UpdateUI();
    }

    private void SetupStatRow(string rowNodeName, string statName)
    {
       var row = FindChild(rowNodeName, true, false);
        var btnAdd = row.FindChild("AddButton", true, false) as Button;
        var btnMinus = row.FindChild("ButtonMinus", true, false) as Button;

        btnAdd.Pressed += () => ChangeStat(statName, 1);
        btnMinus.Pressed += () => ChangeStat(statName, -1);
    }

    private void ChangeStat(string statName, int amount)
    {
        int current = GetStatValue(statName);
        int limit = _baseStats[statName];

        if (amount > 0 && _stats.UnspentPoints > 0)
        {
            SetStatValue(statName, current + 1);
            _stats.UnspentPoints--;
            UpdateUI();
        }
        else if (amount < 0 && current > limit)
        {
            SetStatValue(statName, current - 1);
            _stats.UnspentPoints++;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        _pointsLabel.Text = _stats.UnspentPoints.ToString();

        UpdateStatLabel("StrengthRow", _stats.Strength);
        UpdateStatLabel("AgilityRow", _stats.Agility);
        UpdateStatLabel("IntelligenceRow", _stats.Intelligence);
        UpdateStatLabel("ProtectionRow", _stats.Protection);
        UpdateStatLabel("SpeedRow", _stats.Speed);
        UpdateStatLabel("LuckRow", _stats.Luck);
    }

    private void UpdateStatLabel(string rowName, int value)
    {
        var row = FindChild(rowName, true, false);
        (row.FindChild("Value", true, false) as Label).Text = value.ToString();
    }

    private void OnStartButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Levels/World.tscn");
    }

    private int GetStatValue(string name)
    {
        switch (name)
        {
            case "Strength": return _stats.Strength;
            case "Agility": return _stats.Agility;
            case "Intelligence": return _stats.Intelligence;
            case "Protection": return _stats.Protection;
            case "Speed": return _stats.Speed;
            case "Luck": return _stats.Luck;
            default: return 0;
        }
    }

    private void SetStatValue(string name, int value)
    {
        switch (name)
        {
            case "Strength": _stats.Strength = value; break;
            case "Agility": _stats.Agility = value; break;
            case "Intelligence": _stats.Intelligence = value; break;
            case "Protection": _stats.Protection = value; break;
            case "Speed": _stats.Speed = value; break;
            case "Luck": _stats.Luck = value; break;
        }
    }


}
