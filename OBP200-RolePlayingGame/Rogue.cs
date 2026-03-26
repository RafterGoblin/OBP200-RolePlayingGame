namespace OBP200_RolePlayingGame;

public class Rogue : Player
{
    
    
    public Rogue(string name) : base(name)
    {
        stats.Health = 32; 
        stats.Attack = 8;
        stats.Defence = 3;
        stats.Experience = 0;
        stats.Gold = 20;
    }
}