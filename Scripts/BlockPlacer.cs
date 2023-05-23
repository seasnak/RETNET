using Godot;
using System;
using System.IO;
using System.Collections.Generic;

/*
* DEBUG CLASS -- for testing and seeing how we can place blocks
*/
public partial class BlockPlacer : Node2D 
{

	private TileMap tilemap;
	private Player player;
	private string tilemap_path = "/root/World/Tilemap";
	private string player_path = "/root/World/Player";

	private string start_lvl = "test.txt";

 	// tilemap to world coordinates scale
	private float t2w_scale = 8f;
	private float t2w_offset = 4f;

	public override void _Ready()
	{
		tilemap = GetNode(tilemap_path) as TileMap;

		// instantiate player
		// player = GetNode(player_path) as Player;
		PackedScene player_scene = ResourceLoader.Load<PackedScene>("res://Objects/Player.tscn");
		player = player_scene.Instantiate() as Player;
		
		Camera2D player_camera = new Camera2D();
		player_camera.Zoom = new Vector2(5f, 5f);
		player.AddChild(player_camera);
		this.AddChild(player);

		string os = System.Environment.OSVersion.ToString();
		if(os.Substring(0, 4) == "Unix") {
			BuildLevel($"Levels/{start_lvl}");
		}
		else {
			BuildLevel($"Levels\\{start_lvl}");
		}
		// BuildLevel($"res://Levels/{start_lvl}"); // This doesn't work because file path needs to be absolute -- this is a Godot file path
	}

	public override void _Process(double delta)
	{

	}

	private void SaveLevelToTSCN(string level_fname) {
		// saves the level scene to tscn file
		GD.Print($"saving current level to {level_fname}.tscn");
		
	}

	private Vector2 BuildLevel(string target_fpath, int player_spawn_loc = -1) {
		/*
		Builds a level from <target_fpath> (.txt)
		Places player at "P" block if player_spawn_loc == -1,
			Else places player at target spawn location
		*/
		GD.Print($"Opening File {target_fpath}");
		
		Dictionary<int, string> level_dict = new Dictionary<int, string>();

		int j = 0; // row count for level
		int line_count = 0; // line count in file
		Vector2 dims = new Vector2();
		foreach(string line in File.ReadLines(target_fpath)) {
			line_count++;
			GD.Print($"{line.Length} {line}");
			if(line.Length == 0) { continue; } // empty line -- skip
			else if(line_count == 1) { // line contains level dimensions -- first line
				// GD.Print($"room dimensions: {line[0]}x{line[2]}"); // DEBUG
				var dims_arr = line.Split(" ");
				dims = new Vector2(dims_arr[0].ToFloat(), dims_arr[1].ToFloat());
				continue;
			}
			else if(line[0] == '?') { // line containing links to other levels
				int level_symbol = line.Substring(1, 1).ToInt();
				string target_level_name = line.Substring(3);

				level_dict[level_symbol] = target_level_name; // save filepath to dictionary
				continue;
			}
			
			// not a config line, so loop through line and build level
			string[] blocks = line.Split(" ");
			int i = 0; // col count for level
			foreach(var block in blocks) {
				// GD.Print($"Placing {block} Block"); // DEBUG
				if(block == "B") {
					tilemap.SetCellsTerrainConnect(0, new Godot.Collections.Array<Vector2I> {new Vector2I(i, j)}, 0, 0);
				}
				else { // Place non-tilemap object
					Vector2 obj_pos = new Vector2(i*t2w_scale + t2w_offset, j*t2w_scale + t2w_offset);
					
					if(block == "P" && player_spawn_loc == -1) {
						GD.Print($"Placing Player at position ({j}, {i})");
						player.SetPosition(obj_pos);
					}
					else if(block == "C") {
						GD.Print($"Placing Coin at position ({j}, {i})");
						PackedScene coin_obj = ResourceLoader.Load<PackedScene>("res://Objects/Coin.tscn");
						Coin coin_inst = coin_obj.Instantiate() as Coin;
						coin_inst.SetPosition(obj_pos);

						this.AddChild(coin_inst);
					}
					else if(block.IsValidInt()) { // place a level link to target level
						GD.Print($"Placing Level Link at position ({i}, {j})");
						PackedScene level_link_obj = ResourceLoader.Load<PackedScene>("res://Objects/LevelLink.tscn");
						LevelLink level_link_inst = level_link_obj.Instantiate() as LevelLink;
						
						// want to place the level link slightly off of the level so the player has to "walk into it"
						if(i == 0) {
							obj_pos += new Vector2(-6, 0);
						}
						else if(i == dims[0]-1) {
							obj_pos += new Vector2(6, 0);
						}
						else if(j == 0) {
							obj_pos += new Vector2(0, 6);
						}
						else if(j == dims[1]-1) {
							obj_pos += new Vector2(0, -6);
						}
						level_link_inst.SetPosition(obj_pos);
						GD.Print($"Setting target level to {level_dict[block.ToInt()]}");
						level_link_inst.SetTargetLevel(level_dict[block.ToInt()]);

						this.AddChild(level_link_inst);
					}
				}
				i++;
			}
			j++;
		}

		// Save Level to tscn file for quick loading
		string os = System.Environment.OSVersion.ToString();
		string level_name = "";
		if(os.Substring(0, 4) == "Unix") {
			level_name = target_fpath.Split("/")[^1];
		}
		else {
			level_name = target_fpath.Split("\\")[^1];
		}
		SaveLevelToTSCN(level_name);
		return dims;
	}
}
