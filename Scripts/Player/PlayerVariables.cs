using Godot;
using System;

public partial class PlayerVariables : Node
{

	public static int score = 0;
	
	public static Node2D spawn_point = new Node2D(); // the spawn point for the player
	public static string curr_room = "test.txt";
	public static bool is_new_player = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
