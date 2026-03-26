namespace OBP200_RolePlayingGame;

public abstract class Player
{
    public string ClassType { get; set; }
    public int level { get; set; }
    public int potion { get; set; }
    public string inventory { get; set; }
    public Character stats =  new Character();

    public Player(string name)
    {
        stats.Name = name;
        ClassType = "Player";
        level = 1;
        potion = 1;
    }
}