using Godot;
using System;

public partial class Main : Node2D
{
	bool GameRunning;
	bool GameOver;
	int Scroll;
	int Score;
	const int ScrollSpeed = 4;
	Vector2I ScreenSize;
	int GroundHeight;
	Array Pipes;
	const int PipeDelay = 100;
	const int PipeRange = 200;
	private Bird bird;

	public override void _Ready()
	{
		bird = GetNode<Bird>("Bird");
	}

	public override void _Process(double delta)
	{
	}

	private void NewGame()
	{
		GameRunning = false;
		GameOver = false;
		Score = 0;
		Scroll = 0;
		bird.Reset();
	}

    public override void _Input(InputEvent @event)
	{
		if (!GameOver)
		{
			if (@event is InputEventMouseButton mouseEvent)
			{
				if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
				{
					if (!GameRunning)
					{
						StartGame();
					}
					else
					{
						if (bird.flying)
						{
							bird.Flap();
						}
					}
				}
			}
		}
	}

	public void StartGame()
	{
		GameRunning = true;
		bird.flying = true;
		bird.Flap();
	}
}
