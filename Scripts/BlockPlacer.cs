using Godot;
using System;
using System.IO;

/*
* DEBUG CLASS -- for testing and seeing how we can place blocks
*/
public partial class BlockPlacer : Node2D 
{

	private TileMap tilemap;
	private Player player;
	private string tilemap_path = "/root/World/Tilemap";
	private string player_path = "/root/World/Player";

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


	private Vector2 build_level(string target_fpath) {
		// Build a level from <target_fpath>
		GD.Print($"Opening File {target_fpath}");

		int i = 0; // row count
		Vector2 dims = new Vector2();
		foreach(string line in File.ReadLines(target_fpath)) {
			GD.Print($"{line.Length} {line}");
			if(line.Length == 0) { continue; } // empty line
			else if(line.Length == 3) { // line contains level dimensions
				GD.Print($"room dimensions: {line[0]}x{line[2]}");
				dims = new Vector2(line[0], line[2]);
				continue;
			}

			string[] blocks = line.Split(" ");
			int j = 0;
			foreach(var block in blocks) {
				GD.Print($"placing {block} block"); // DEBUG
				if(block == "B") {
					// tilemap.SetCellsTerrainConnect(0, new Godot.Collections.Array<Vector2I> {new Vector2I(i-2, j)}, 0, 0);
					tilemap.SetCellsTerrainConnect(
						0, 
						new Godot.Collections.Array<Vector2I> {new Vector2I(j, i)},
						0, 0
					);
				}
				else if(block == "P") {
					GD.Print($"placing player at position ({j}, {i})");
					player.set_position(new Vector2(j*6 + 6, i*6 + 2));
				}
				else if(block == "")

				j++;
			}
			
			i++;
		}

		return dims;
	}
}
