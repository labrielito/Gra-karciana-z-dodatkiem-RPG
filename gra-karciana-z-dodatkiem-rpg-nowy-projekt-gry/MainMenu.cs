using Godot;
using System;



public partial class MainMenu : Control
{
	private Button StartButton;
	private Button OptionsButton;
	private Button QuitButton;
	private AudioStreamPlayer BackgroundMusic;

	public override void _Ready()
	{
		BackgroundMusic =GetNode<AudioStreamPlayer>("BackgroundMusic");
		QuitButton = GetNode<Button>("HBoxContainer/Quit");
		QuitButton.Pressed += OnQuitButtonPressed;
 
		PlayBackgroundMusic();
	}

	private void PlayBackgroundMusic()
	{
		if (BackgroundMusic != null && !BackgroundMusic.Playing)
		{
			BackgroundMusic.Play();
		}
	}



	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}
