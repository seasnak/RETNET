namespace RetNet;

public partial class Coin : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		try
		{
			sprite.Play("Default");
		}
		catch (System.Exception)
		{
			GD.Print("Error: Coin has no animation!");
			throw;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
