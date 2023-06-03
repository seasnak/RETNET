using Godot;
using System;

public partial class LevelLink : Area2D
{
	private string target_level = "test.txt"; // set default level to test.txt
	// Called when the node enters the scene tree for the first time.
	
	public override void _Ready() {
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		
	}

	public void SetPosition(Vector2 position) {
		this.Position = position;
	}

	private void OnBodyEntered(Node2D body) {

		if(body is Player) {
			GD.Print($"Collided With Player -- Loading Level: {target_level}");
			// Load Level

			// if(FileAccess.FileExists($"res://Scenes/{target_level}")) { // if level file already exists, then just load that instead of building it from scratch
			// 	GetTree().ChangeSceneToFile($"res://Scenes/{target_level}");
			// 	return;
			// }
			string target_scene = target_level.Substr(0, target_level.Length-4);

			GetTree().ChangeSceneToFile("res://Scenes/template.tscn");
			BlockPlacer temp_bp = GetNode<BlockPlacer>("/root/World/BlockPlacer");
			temp_bp.BuildLevel(target_level);
			temp_bp.SaveLevelToTSCN(target_scene);

			// var load_success = GetTree().ChangeSceneToFile($"res://Scenes/{target_scene}.tscn");
			// GD.Print(load_success); // DEBUG
			BlockPlacer bp = GetNode<BlockPlacer>("/root/World/BlockPlacer");
			LevelLink player_spawn_obj = GetNode<LevelLink>($"/root/World/{PlayerVariables.curr_room}");
			GD.Print(player_spawn_obj.Name);
			PlayerVariables.curr_room = target_level;
			GD.Print(PlayerVariables.curr_room);

			Player player = GetNode<Player>("/root/World/Player");
			bp.InstantiateCamera(player);
			player.SetPosition(player_spawn_obj.Position);
		}
	}

	public void SetTargetLevel(string target) {
		this.target_level = target;
	}
	
}

