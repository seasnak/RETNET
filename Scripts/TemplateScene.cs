using Godot;

public partial class TemplateScene : Node2D {
	private TileMap tilemap;
	private BlockPlacer bp;


	public override void _Ready() {
		tilemap = ResourceLoader.Load<TileMap>("res://Objects/tilemap.tscn");
		this.AddChild(tilemap);

		bp = ResourceLoader.Load<BlockPlacer>("res://Objects/block_placer.tscn");
		this.AddChild(bp);

		PlayerVariables player_vars = GetNode<PlayerVariables>("res://Scripts/Player/PlayerVariables.cs");
	}

	public override void _Process(double delta) {
		// bp.BuildLevel();
		
	}

}