using Godot;
using System;

public partial class Main : Node2D
{
	[Export]
	public PackedScene PipeScene;
	bool GameRunning;
	bool GameOver;
	int Scroll;
	int Score;
	const int ScrollSpeed = 4;
	Vector2I ScreenSize;
	int GroundHeight;
	private Godot.Collections.Array<Pipe> Pipes = new();
	const int PipeDelay = 100;
	const int PipeRange = 200;
	private Bird bird;
	private Ground ground;
	private Random r = new Random();

	public override void _Ready()
	{
		ScreenSize = GetWindow().Size;
		bird = GetNode<Bird>("Bird");
		ground = GetNode<Ground>("Ground");
		GroundHeight = ground.GetNode<Sprite2D>("Sprite2D").Texture.GetHeight();
		GetNode<Timer>("PipeTimer").Timeout += _OnPipeTimerTimeout;
		ground.Hit += _OnGroundHit;
		GetNode<GameOver>("GameOver").Restart += _OnGameOverRestart;
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
			
			foreach(Pipe pipe in Pipes)
			{
				Vector2 Pos = pipe.Position;
				Pos.X -= ScrollSpeed;
				pipe.Position = Pos;
			}
		}
	}

	private void NewGame()
	{
		GameRunning = false;
		GameOver = false;
		Score = 0;
		Scroll = 0;
		GetNode<Label>("ScoreLabel").Text = "Score: " + Score.ToString();
		GetNode<CanvasLayer>("GameOver").Hide();

		foreach(Pipe pipe in Pipes)
		{
			pipe.QueueFree();
		}
		Pipes.Clear();
		GeneratePipes();
		//GetNode<Timer>("PipeTimer").Start();
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
							CheckTop();
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
		GetNode<Timer>("PipeTimer").Start();
	}

	public void _OnPipeTimerTimeout()
	{
		GeneratePipes();
	}

	public void GeneratePipes()
	{
		Pipe pipe = PipeScene.Instantiate<Pipe>();
		Vector2 Pos = pipe.Position;
		Pos.X = ScreenSize.X + PipeDelay;
		Pos.Y = (ScreenSize.Y - GroundHeight)/2 + r.Next(-PipeRange, PipeRange);
		pipe.Position = Pos;
		pipe.Hit += BirdHit;
		pipe.Scored += Scored;
		AddChild(pipe);
		Pipes.Add(pipe);
	}

	public void Scored()
	{
		Score += 1;
		GetNode<Label>("ScoreLabel").Text = "Score: " + Score.ToString();
	}

	public void CheckTop()
	{
		if(bird.Position.Y < 0)
		{
			bird.falling = true;
			StopGame();
		}
	}

	public void StopGame()
	{
		GetNode<Timer>("PipeTimer").Stop();
		GetNode<CanvasLayer>("GameOver").Show();
		bird.flying = false;
		GameRunning = false;
		GameOver = true;
	}

	public void BirdHit()
	{
		bird.falling = true;
		StopGame();
	}

	public void _OnGroundHit()
	{
		bird.falling = true;
		StopGame();
	}

	public void _OnGameOverRestart()
	{
		NewGame();
	}
}
