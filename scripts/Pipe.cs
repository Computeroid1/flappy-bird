using Godot;
using System;

public partial class Pipe : Area2D
{
	[Signal]
	public delegate void HitEventHandler();

	[Signal]
	public delegate void ScoredEventHandler();

    public override void _Ready()
    {
        BodyEntered += _OnBodyEntered;
		GetNode<Area2D>("ScoreArea").BodyEntered += _OnScoreAreaBodyEntered;
    }

	public void _OnBodyEntered(Node2D body)
	{
		EmitSignal(SignalName.Hit);
	}

	public void _OnScoreAreaBodyEntered(Node2D body)
	{
		EmitSignal(SignalName.Scored);
	}
}
