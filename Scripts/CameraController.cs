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
		if(this.Position != player.Position) {
			Vector2 move_pos = new Vector2(player.Position[0], 0);

			this.Position = move_pos;
		}
	}

	public void set_player(Player p) {
		this.player = p;
	}
}
