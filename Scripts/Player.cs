using Godot;
using System;

public partial class Player : CharacterBody2D
{
	
	private int max_health = 200;
	private int curr_health;
	private int max_stamina = 200;
	private int curr_stamina;

	private int movespeed = 10; // the player's max movespeed
	private float acceleration = 0.2f; // the player's acceleration from resting
	private int jumpspeed = 15; // the players jumping speed
	
	
	private bool is_grounded = true; // groundcheck
	private bool is_jumping = false;  // jump check
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{	
		if(curr_health <= 0) { Die(); } // check death
		
		// MOVEMENT

		is_grounded = this.IsOnFloor();
		
		if(Input.IsActionJustPressed("ui_left")) {
			Velocity = new Vector2(Math.Min(Velocity.X + acceleration, movespeed), Velocity.Y);
		}
		if(Input.IsActionJustPressed("ui_right")) {
			Velocity = new Vector2(Math.Max(Velocity.X-acceleration, movespeed), Velocity.Y);
		}
		
		MoveAndSlide();
	}

	private void Die() {
		GD.Print("Player Died!");
	}
		
}
