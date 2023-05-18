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

	private float block_to_level_scale = 9.2f;

	public override void _Ready()
	{
		tilemap = GetNode(tilemap_path) as TileMap;
		
		
		// Placing Blocks No Autotile
		// tilemap.SetCell(0, new Vector2I(1, 1), 0, new Vector2I(1, 1));
		// tilemap.SetCell(0, new Vector2I(1, 2), 0, new Vector2I(1, 1));

		// Placing Blocks Autotile
		// Godot.Collections.Array<Vector2I> tile_coords = new Godot.Collections.Array<Vector2I> { 
		// 	new Vector2I(1, 1), new Vector2I(1, 2)
		// };

		// tilemap.SetCellsTerrainConnect(0, tile_coords, 0, 0);
		// tilemap.SetCellsTerrainConnect(0, new Godot.Collections.Array<Vector2I> {new Vector2I(1, 1)}, 0, 0);

		// tilemap.SetCellsTerrainConnect(0, new Godot.Collections.Array<Vector2I> {new Vector2I(1, 2)}, 0, 0);

		player = GetNode(player_path) as Player;
		GD.Print($"Current Directory: {Directory.GetCurrentDirectory()}"); // DEBUG

		string os = System.Environment.OSVersion.ToString();
		// GD.Print($"Operation System: {os.Substring(0, 4)}"); // DEBUG
		if(os.Substring(0, 4) == "Unix") {
			build_level("Levels/test.txt");
		}
		else {
			build_level("Levels\\test.txt");
		}
		
	}

	public override void _Process(double delta)
	{

	}

	private Vector2 build_level(string target_fpath, int player_loc = -1) {
		// Build a level from <target_fpath>
		GD.Print($"Opening File {target_fpath}");
		
		Dictionary<int, string> level_dict = new Dictionary<int, string>();

		int i = 0; // row count
		int count = 0;
		Vector2 dims = new Vector2();
		foreach(string line in File.ReadLines(target_fpath)) {
			count++;
			GD.Print($"{line.Length} {line}");
			if(line.Length == 0) { continue; } // empty line
			else if(count == 1) { // line contains level dimensions -- first line
				GD.Print($"room dimensions: {line[0]}x{line[2]}");
				dims = new Vector2(line[0], line[2]);
				continue;
			}
			else if(line[0] == '?') { // line containing links to other levels
				int level_symbol = line.Substring(1, 1).ToInt();
				string level_name = line.Substring(3);

				level_dict[level_symbol] = level_name; // save filepath to dictionary
				continue;
			}
			
			// not a config line, so loop through line and build level
			string[] blocks = line.Split(" ");
			int j = 0;
			foreach(var block in blocks) {
				GD.Print($"Placing {block} Block"); // DEBUG
				if(block == "B") {
					tilemap.SetCellsTerrainConnect(0, new Godot.Collections.Array<Vector2I> {new Vector2I(j, i)}, 0, 0);
				}
				else if(block == "P") {
					GD.Print($"Placing Player at position ({j}, {i})");
					player.SetPosition(new Vector2(j*9.2f, i*9.2f));
				}
				else if(block == "C") {
					GD.Print($"Placing Coin at position ({j}, {i})");
					PackedScene coin_obj = ResourceLoader.Load<PackedScene>("res://Objects/Coin.tscn");
					Coin coin_inst = coin_obj.Instantiate() as Coin;
					coin_inst.SetPosition(new Vector2(j*9.2f, i*9.2f));

					// this.GetParent().AddChild(coin_instance);
					this.AddChild(coin_inst);
				}
				else if(block.IsValidInt()) {
					GD.Print($"Placing Level Link at position ({j}, {i})");
					PackedScene level_link_obj = ResourceLoader.Load<PackedScene>("res://Objects/LevelLink.tscn");
					LevelLink level_link_inst = level_link_obj.Instantiate() as LevelLink;
					
					level_link_inst.SetPosition(new Vector2(j*9.2f, i*9.2f));
					level_link_inst.SetTargetLevel(level_dict[block.ToInt()]);
					this.AddChild(level_link_inst);
				}
				j++;
			}
			i++;
		}

		return dims;
	}
}
