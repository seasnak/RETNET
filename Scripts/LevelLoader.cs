using Godot;
using System;
using System.IO;

public partial class LevelLoader : Node2D
{

    string target_fname = "../Levels/test.txt";

    public override void _Ready()
    {
        
    }

    public override void _Process(double delta)
    {
        
    }

    public void LoadLevelFromText(string level_fname) {
        if(!File.Exists(level_fname)) {
            GD.PrintErr("Level Error: Level File Not Found");
        }

        using(StreamReader sr = File.OpenText(level_fname)) {
            string s;
            while((s = sr.ReadLine())!= null) {
                // Generate Level Here based on text file contents
                string[] blocks = s.Split(' ');
                for(int i = 0; i < blocks.Length; i++) {
                    GD.Print(blocks[i][0]); // print out the block at the location
                }
                
                GD.Print(s);
            }
        }
    }
}