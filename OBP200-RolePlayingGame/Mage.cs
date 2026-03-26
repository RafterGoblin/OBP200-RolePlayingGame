namespace OBP200_RolePlayingGame;

public class Mage : Player
{

    public Mage(string Name) : base(Name)
    {
        stats.Health = 28; 
        stats.Attack = 10;
        stats.Defence = 2;
        stats.Experience = 0;
        stats.Gold = 15;
    }
}