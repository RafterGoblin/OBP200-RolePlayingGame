namespace OBP200_RolePlayingGame;

public class Warrior : Player
{
   
    public Warrior(string name) : base(name)
    {
        stats.Health = 40; 
        stats.MaxHealth = 40;
        stats.Attack = 7;
        stats.Defence = 5;
        stats.Experience = 0;
        stats.Gold = 15;
        potion = 2;
    }
    
    public override int SpecialAttack(int attack, int enemyDefence,   Random Rng)
    {
        int specialDmg = 0;
        Console.WriteLine("Warrior använder Heavy Strike!");
        specialDmg = Math.Max(2, stats.Attack + 3 - enemyDefence);
        stats.Health -= 2;
        return specialDmg;
    }
}