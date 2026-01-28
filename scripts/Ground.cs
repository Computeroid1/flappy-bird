using Godot;
using System;

public partial class Ground : Area2D
{
	[Signal]
	public delegate void HitEventHandler();

    public override void _Ready()
    {
        BodyEntered += _OnBodyEntered;
    }

	public void _OnBodyEntered(Node2D body)
	{
		EmitSignal(SignalName.Hit);
	}
}
