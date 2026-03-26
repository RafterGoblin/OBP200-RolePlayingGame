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
        potion = 2;
    }

    public override int SpecialAttack(int attack, int enemyDefence,  Random Rng)
    {
        int specialDmg;
        if (stats.Gold >= 3)
        {
            Console.WriteLine("Mage kastar Fireball!");
            stats.Gold = (stats.Gold - 3);
            specialDmg = Math.Max(3, stats.Attack + 5 - (enemyDefence / 2));
        }
        else
        {
            Console.WriteLine("Inte tillräckligt med guld för att kasta Fireball (kostar 3).");
            specialDmg = 0;
        }
        return specialDmg;
    }
}