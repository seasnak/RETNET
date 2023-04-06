using Godot;
using System;

public partial class Player : Node
{
	
	private int max_health = 200;
	private int curr_health;
	private int max_stamina = 200;
	private int curr_stamina;

	private int movespeed = 10; // the player's max movespeed
	private float acceleration = 0.2f; // the player's acceleration from resting
	private int jumpspeed = 15; // the players jumping speed
	private Vector2 velocity = new Vector2(); // the Vector2 representation of the players movements
	
	private bool is_grounded = true; // groundcheck
	private bool is_jumping = false;  // jump check

	private AnimatedSprite2D sprite;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{	
		if(curr_health > max_health) { curr_health = max_health; } // check max health (remove later)
		else if(curr_health <= 0) { QueueFree(); } // check death
		
		// MOVEMENT
		float plr_input_hor = Input.GetAxis("neg_hor", "pos_hor");
		if(plr_input_hor > 0) {
			velocity.X = Math.Min(movespeed, velocity.X + acceleration);
		}
		else if(plr_input_hor < 0 ) {
			
			velocity.X = Math.Max(-movespeed, velocity.X - acceleration);
		}

		
	}
		
}
