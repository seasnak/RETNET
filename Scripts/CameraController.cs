using Godot;
using System;
// TODO: In future, maybe make a more dynamic camera but this is more stylistic

public partial class CameraController : Camera2D
{
	
	Player player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		this.Position = player.Position;
	}

	public void set_player(Player p) {
		this.player = p;
	}
}
