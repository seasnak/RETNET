using Godot;
using System;

public partial class Coin : Area2D
{

	private int value = 1; // the value of this coin

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// AnimatedSprite2D sprite = (AnimatedSprite2D)(GetNode("AnimatedSprite2D"));
		// try {
		// 	sprite.Play("default");
		// }
		// catch (System.Exception) {
		// 	GD.Print("Error: Coin has no animation!");
		// 	throw;
		// }

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	private void OnBodyEntered(Node2D body) {
		// GD.Print($"{body} has entered object"); // DEBUG
		if(body is Player) { // player can pick up coin
			Player player = (Player)body;
			player.AddCurrency(value);
			
			QueueFree();
		}
	}

	

	public void SetPosition(Vector2 position) {
		this.Position = position;
	}
}
