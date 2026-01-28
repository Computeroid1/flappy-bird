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
		AddChild(pipe);
		Pipes.Add(pipe);
	}

	public void BirdHit()
	{
		
	}
}
