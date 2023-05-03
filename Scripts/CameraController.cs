namespace RetNet;

// TODO: In future, maybe make a more dynamic camera but this is more stylistic
public partial class CameraController : Camera2D
{
	
	CharacterBody2D player_body;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
			
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		this.Position = player_body.Position;
	}
}
