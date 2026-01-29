using Godot;
using System;

[GlobalClass]
public partial class CharacterStats : Resource
{
    [Export] public string ClassName = "Klasa";
    [Export] public Texture2D ClassIcon; 

    [ExportGroup("Główne Statystyki")]
    [Export] public int Strength = 10;      
    [Export] public int Agility = 5;        
    [Export] public int Intelligence = 5;   
    [Export] public int Protection = 5;     
    [Export] public int Speed = 10;         
    [Export] public int Luck = 5;           

    [ExportGroup("Życie i Mana (Skalowanie)")]
    [Export] public int BaseHealth = 100;
    [Export] public int BaseMana = 20;

    [Export] public int HealthPerLevel = 10;
    [Export] public int ManaPerLevel = 5;

    [ExportGroup("Postęp")]
    [Export] public int Level = 1;
    [Export] public int Experience = 0;
    [Export] public int UnspentPoints = 0;
    
    public int CurrentHealth;
    public int CurrentMana;

    public int GetMaxHealth()
    {
        return BaseHealth + (HealthPerLevel * (Level - 1));
    }

    public int GetMaxMana()
    {
        return BaseMana + (ManaPerLevel * (Level - 1));
    }

}
