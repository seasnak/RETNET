using Godot;
using System;

public partial class LevelLink : Area2D
{
	private string target_level = "res://Levels/test.txt"; // set default level to test.txt
	// Called when the node enters the scene tree for the first time.
	
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void SetPosition(Vector2 position) {
		this.Position = position;
	}

	private void OnBodyEntered(Node2D body) {

		if(body is Player) {
			GD.Print($"Collided With Player -- Loading Level: {target_level}");
			// Load Level
			PackedScene target_scene = GD.Load<PackedScene>(target_level);
		}
	}

	public void SetTargetLevel(string target) {
		this.target_level = target;
	}
	
}

