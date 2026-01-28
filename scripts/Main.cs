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
	private Ground ground;

	public override void _Ready()
	{
		ScreenSize = GetWindow().Size;
		bird = GetNode<Bird>("Bird");
		ground = GetNode<Ground>("Ground");
		NewGame();
	}

	public override void _Process(double delta)
	{
		if (GameRunning)
		{
			Scroll += ScrollSpeed;

			if(Scroll >= ScreenSize.X)
			{
				Scroll = 0;
			}
			ground.Position = new Vector2(-Scroll, ground.Position.Y);
		}
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
