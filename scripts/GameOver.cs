using Godot;
using System;

public partial class GameOver : CanvasLayer
{
	[Signal]
	public delegate void RestartEventHandler();

    public override void _Ready()
    {
        GetNode<Button>("RestartButton").Pressed += _OnRestartButtonPressed;
    }

	public void _OnRestartButtonPressed()
	{
		EmitSignal(SignalName.Restart);
	}
}
