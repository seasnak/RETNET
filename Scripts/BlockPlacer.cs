using Godot;
using System;

/*
* DEBUG CLASS -- for testing and seeing how we can place blocks
*
*/
public partial class BlockPlacer : Node2D 
{

    private TileMap tilemap;
    private string tilemap_path = "/root/World/Tilemap";

    public override void _Ready()
    {   
        // DEBUG: print out paths of all children
        // var children = this.GetParent().GetChildren();
        
        // // GD.Print( children.GetType() );  
        // for (int i = 0; i < children.Count; i++) {
        //     GD.Print(children[i].GetPath());
        // }
        tilemap = GetNode(tilemap_path) as TileMap;
        
        var tile_coords = [new Vector2I(1, 1), new Vector2I(1, 2)] as Godot.Collections.Array<Vector2I>;
        tilemap.SetCellsTerrainConnect(0,  , 0, 0);
        
        // tilemap.SetCell(0, new Vector2I(1, 1), 0, new Vector2I(1, 1));
        // tilemap.SetCell(0, new Vector2I(1, 2), 0, new Vector2I(1, 1));
        
    }

    public override void _Process(double delta)
    {
        
    }

}