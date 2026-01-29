using Godot;
using System;
using System.Threading.Tasks; // Potrzebne do opóźnienia tury wroga

public partial class BattleScene : Node2D 
{
    // --- UI ---
    [ExportGroup("Gracz (Lewa Strona)")]
    [Export] public Label PlayerHPLabel;
    [Export] public Label PlayerStaminaLabel;
    [Export] public Label VictoryLabel;
    [Export] public TextureRect PlayerImage;

    [ExportGroup("Przeciwnik (Prawa Strona)")]
    [Export] public Label EnemyHPLabel;
    [Export] public Label EnemyStatsLabel;
    [Export] public TextureRect EnemyImage;

    [ExportGroup("Karty (Przyciski)")]
    [Export] public Button CardAttackButton; 
    [Export] public Button CardSkill1Button; 
    [Export] public Button CardSkill2Button; 

    // --- Dane ---
    public CharacterStats PlayerStats; 
    public CharacterStats EnemyStats;  

    private int _currentStamina = 3;
    private int _maxStamina = 3;
    private bool _isPlayerTurn = true; 

    public override void _Ready()
    {

        SetButtonsState(false);

        if (CardAttackButton != null)
            CardAttackButton.Pressed += OnAttackButton_Pressed;

        if (CardSkill1Button != null)
            CardSkill1Button.Pressed += OnSkill1Button_Pressed;

        if (CardSkill2Button != null)
            CardSkill2Button.Pressed += OnSkill2Button_Pressed;

        if (GameManager.Instance != null && 
            GameManager.Instance.PlayerStatsPending != null && 
            GameManager.Instance.EnemyStatsPending != null)
        {
            SetupBattle(
                GameManager.Instance.PlayerStatsPending, 
                GameManager.Instance.EnemyStatsPending
            );
            GameManager.Instance.EnemyStatsPending = null; 
        }
        else
        {
            GD.Print("Uruchomiono scenę walki bez danych (testowanie F6).");
        }
    }

    public void SetupBattle(CharacterStats player, CharacterStats enemy)
    {
        PlayerStats = player; 
        EnemyStats = (CharacterStats)enemy.Duplicate(); 
        EnemyStats.CurrentHealth = EnemyStats.GetMaxHealth(); 

        PlayerStats.CurrentHealth = PlayerStats.GetMaxHealth();
        
        _currentStamina = _maxStamina;
        _isPlayerTurn = true;

        UpdateUI();
        SetButtonsState(true);
    }

    private void OnAttackButton_Pressed()
    {
        if (!_isPlayerTurn) return;

        int damage = PlayerStats.Strength;
        DealDamageToEnemy(damage);

        if (_currentStamina < _maxStamina)
        {
            _currentStamina++;
        }
        
        EndPlayerAction();
    }

    private void OnSkill1Button_Pressed()
    {
        if (!_isPlayerTurn) return;

        int cost = 2;
        if (_currentStamina < cost) return; 

        _currentStamina -= cost; 
        
        int damage = PlayerStats.Strength * 2; 
        DealDamageToEnemy(damage);

        EndPlayerAction();
    }

    private void OnSkill2Button_Pressed()
    {
        if (!_isPlayerTurn) return;

        int cost = 3;
        if (_currentStamina < cost) return;

        _currentStamina -= cost;

        int healAmount = 20 + (PlayerStats.Intelligence * 2);
        PlayerStats.CurrentHealth += healAmount;
        if (PlayerStats.CurrentHealth > PlayerStats.GetMaxHealth()) 
            PlayerStats.CurrentHealth = PlayerStats.GetMaxHealth();
        
        GD.Print($"Uleczyłeś się o {healAmount} HP!");

        EndPlayerAction();
    }

    private void DealDamageToEnemy(int damage)
    {
        int finalDamage = Math.Max(1, damage - EnemyStats.Protection);
        
        EnemyStats.CurrentHealth -= finalDamage;
        GD.Print($"Zadałeś {finalDamage} obrażeń!");

        if (EnemyStats.CurrentHealth <= 0)
        {
            WinBattle();
        }
    }

    private void EndPlayerAction()
    {
        UpdateUI();

        if (EnemyStats.CurrentHealth <= 0) return;

        StartEnemyTurn();
    }

    private async void StartEnemyTurn()
    {
        _isPlayerTurn = false;
        SetButtonsState(false); 
        
        GD.Print("Tura przeciwnika...");
        
        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");

        int damage = EnemyStats.Strength;
        int finalDamage = Math.Max(1, damage - PlayerStats.Protection);
        
        PlayerStats.CurrentHealth -= finalDamage;
        GD.Print($"Przeciwnik zadał Ci {finalDamage} obrażeń!");

        UpdateUI();

        if (PlayerStats.CurrentHealth <= 0)
        {
            LoseBattle();
        }
        else
        {
            _isPlayerTurn = true;
            SetButtonsState(true); 
            GD.Print("Twoja tura!");
        }
    }

    private void WinBattle()
    {
        
        PlayerStats.Experience += 50; 
        
        if (VictoryLabel != null)
    {
        VictoryLabel.Text = "ZWYCIĘSTWO!"; 
        VictoryLabel.Visible = true;       
    }

    var timer = GetTree().CreateTimer(2.0f);
    timer.Timeout += () => GetTree().ChangeSceneToFile("res://Scenes/Levels/World.tscn");
    }

    private void LoseBattle()
    {
        GD.Print("PORAŻKA...");
        GetTree().ChangeSceneToFile("res://Scenes/gui/DeathScreen.tscn");
    }

    private void UpdateUI()
    {
        if (PlayerStats != null)
        {
            PlayerHPLabel.Text = $"HP: {PlayerStats.CurrentHealth}/{PlayerStats.GetMaxHealth()}";
            PlayerStaminaLabel.Text = $"Stamina: {_currentStamina}/{_maxStamina}";
        }
        
        if (EnemyStats != null)
        {
            EnemyHPLabel.Text = $"HP: {EnemyStats.CurrentHealth}/{EnemyStats.GetMaxHealth()}";
            EnemyStatsLabel.Text = $"Siła: {EnemyStats.Strength}\nObrona: {EnemyStats.Protection}\nSzczęście: {EnemyStats.Luck}\nZręczność: {EnemyStats.Agility}";
        }

        if (_isPlayerTurn)
        {
            CardSkill1Button.Disabled = _currentStamina < 2;
            CardSkill2Button.Disabled = _currentStamina < 3;
        }
    }

    private void SetButtonsState(bool enabled)
    {
        CardAttackButton.Disabled = !enabled;
        CardSkill1Button.Disabled = !enabled;
        CardSkill2Button.Disabled = !enabled;
    }
}