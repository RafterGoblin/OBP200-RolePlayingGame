namespace OBP200_RolePlayingGame;

public class Warrior : Player
{
   
    public Warrior(string name) : base(name)
    {
        stats.Health = 40; 
        stats.Attack = 7;
        stats.Defence = 5;
        stats.Experience = 0;
        stats.Gold = 15;

    }
}