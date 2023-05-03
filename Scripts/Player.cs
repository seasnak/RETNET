// Maybe move these global usings to Globals.cs or RetNet.cs or GameMaster.cs later
global using Godot;
global using System;

namespace RetNet;

public partial class Player : CharacterBody2D
{
	
	private int max_health = 3;
	private int curr_health = 3;
	private int max_stamina = 20;
	private int curr_stamina = 20;

	
	private int movespeed = 65; // the player's max movespeed
	private float acceleration = 0.2f; // TODO: add in some form of acceleration
	private int low_jumpspeed = 45; // the minimum players jumping speed
	private int high_jumpspeed = 100; // the maximum player jumping speed
	private int fast_fallspeed = 30; // allows for better player jump movement
	
	private bool is_grounded = true; // groundcheck
	private bool is_held_jump = false;  // jump check
	private bool has_fastfell = false; // fast fall var for better jump
	
	private int gravity = 200;
	private int max_fallspeed = 200;


	private AnimatedSprite2D sprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = (AnimatedSprite2D)(GetNode("AnimatedSprite2D"));
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (Velocity.Y < max_fallspeed) {
			Velocity += new Vector2( 0, (float)(gravity * delta) );
		}
		else {
			Velocity = new Vector2( Velocity.X, Math.Min(Velocity.Y, max_fallspeed) );
		}

		if(curr_health <= 0) { die(); } // check death
		
		// MOVEMENT
		handle_move();

		// JUMP MOVEMENT
		handle_jump();
	}

	private void die() {
		GD.Print("Player Died!");
	}

	private void handle_move() {
		float hor_input = Input.GetAxis("ui_left", "ui_right");
		Velocity = new Vector2(movespeed * hor_input, Velocity.Y);

		if(hor_input > 0) { // Player is moving right
			// Flip the Character
			sprite.FlipH = false;
			sprite.Play("Walk");
		}
		else if(hor_input < 0) { // Player is moving left
			// Flip the Character 
			sprite.FlipH = true;
			sprite.Play("Walk");
		}
		else {
			sprite.Play("Idle");
		}
	}

	private void handle_jump() {
		is_grounded = this.IsOnFloor();
		// GD.Print(Velocity);
		if (is_grounded) {
			has_fastfell = false;
			if (Input.IsActionJustPressed("Jump")) {
				Velocity = new Vector2(Velocity.X, -high_jumpspeed);
				is_held_jump = true;
			}
		}
		else {
			if (Input.IsActionJustReleased("Jump") && Velocity.Y < -low_jumpspeed) {
				// GD.Print("Player Released Jump Key");
				Velocity = new Vector2(Velocity.X, -low_jumpspeed);
				// so that the player doesn't continuously jump while holding down the jump button
				is_held_jump = false;
			}

			if (this.Velocity.Y > 0 && !has_fastfell) {
				this.Velocity += new Vector2(0, fast_fallspeed);
				has_fastfell = true;
			}
		}
		
		MoveAndSlide();
	}
		
}
