namespace RetNet;

public partial class Enemy : CharacterBody2D 
{

    private int max_health = 3;
    private int curr_health = 3;
    
    public override void _Ready()
    {
        
    }

    public override void _Process(double delta)
    {
        
        // CHECK DEATH
        if (curr_health <= 0) { QueueFree(); }

    }

}