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

	private void SaveLevel(string level_fname) {
		// saves the level to tscn file as a scene
		GD.Print(level_fname);
		
	}

	private Vector2 BuildLevel(string target_fpath, int player_loc = -1) {
		// Build a level from <target_fpath>
		GD.Print($"Opening File {target_fpath}");
		
		Dictionary<int, string> level_dict = new Dictionary<int, string>();

		int i = 0; // row count for level
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
			int j = 0; // col count for level
			foreach(var block in blocks) {
				// GD.Print($"Placing {block} Block"); // DEBUG
				if(block == "B") {
					tilemap.SetCellsTerrainConnect(0, new Godot.Collections.Array<Vector2I> {new Vector2I(j, i)}, 0, 0);
				}
				else if(block == "P") {
					GD.Print($"Placing Player at position ({j}, {i})");
					player.SetPosition(new Vector2(j*t2w_scale + t2w_offset, i*t2w_scale + t2w_offset));
				}
				else if(block == "C") {
					GD.Print($"Placing Coin at position ({j}, {i})");
					PackedScene coin_obj = ResourceLoader.Load<PackedScene>("res://Objects/Coin.tscn");
					Coin coin_inst = coin_obj.Instantiate() as Coin;
					coin_inst.SetPosition(new Vector2(j*t2w_scale + t2w_offset, i*t2w_scale + t2w_offset));

					this.AddChild(coin_inst);
				}
				else if(block.IsValidInt()) { // place a level link to target level
					GD.Print($"Placing Level Link at position ({j}, {i})");
					PackedScene level_link_obj = ResourceLoader.Load<PackedScene>("res://Objects/LevelLink.tscn");
					LevelLink level_link_inst = level_link_obj.Instantiate() as LevelLink;
					
					level_link_inst.SetPosition(new Vector2(j*t2w_scale + t2w_offset, i*t2w_scale + t2w_offset));
					GD.Print($"Setting target level to {level_dict[block.ToInt()]}");
					level_link_inst.SetTargetLevel(level_dict[block.ToInt()]);

					this.AddChild(level_link_inst);
				}
				j++;
			}
			i++;
		}

		string os = System.Environment.OSVersion.ToString();
		string level_name = "";
		if(os.Substring(0, 4) == "Unix") {
			level_name = target_fpath.Split("/")[^1];
		}
		else {
			level_name = target_fpath.Split("\\")[^1];
		}
		SaveLevel(level_name);
		return dims;
	}
}
