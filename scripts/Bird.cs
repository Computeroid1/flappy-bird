using Godot;
using System;

public partial class Bird : CharacterBody2D
{
	const float GRAVITY = 1000;
	const float MAX_VEL = 600;
	const float FLAP_SPEED = -500;
	public bool flying = false;
	bool falling = false;
	static readonly Vector2 START_POS = new Vector2(100, 400);

	public override void _Ready()
	{
		Reset();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (flying || falling)
		{
			Vector2 vel = Velocity;
			vel.Y += (float)(GRAVITY * delta);

			if(vel.Y > MAX_VEL)
			{
				vel.Y = MAX_VEL;
			}

			Velocity = vel;

			if(flying)
			{
				SetRotation((float)Double.DegreesToRadians(Velocity.Y * 0.05));
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play();
			} else if(falling)
			{
				SetRotation((float)Math.PI/2);
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Stop();
			}
			MoveAndSlide();
		} else
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Stop();
		}
	}

	public void Reset()
	{
		falling = false;
		flying = false;
		Position = START_POS;
		SetRotation(0);
	}

	public void Flap()
	{
		Vector2 vel = Velocity;
		vel.Y = FLAP_SPEED;
		Velocity = vel;
	}
}
