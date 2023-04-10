using Godot;
using System;

public partial class Player : CharacterBody2D
{
	
	private int max_health = 200;
	private int curr_health;
	private int max_stamina = 200;
	private int curr_stamina;

	private int movespeed = 10; // the player's max movespeed
	private float acceleration = 0.2f; // TODO: add in some form of acceleration
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
		
		float hor_input = Input.GetAxis("ui_left", "ui_right");
		Velocity = new Vector2(movespeed * hor_input, Velocity.Y);
		if(hor_input > 0) { // Player is moving right
			this.Scale = new Vector2(1, 1);
		}
		else if(hor_input < 0) { // Player is moving left
			this.Scale = new Vector2(-1, 1);
		}
		
		MoveAndSlide();
	}

	private void Die() {
		GD.Print("Player Died!");
	}
		
}
