namespace OBP200_RolePlayingGame;

public class Mage : Player, IAttack
{

    
    public Mage(string Name) : base(Name)
    {
        stats.Health = 28; 
        stats.MaxHealth = 28;
        stats.Attack = 10;
        stats.Defence = 2;
        stats.Experience = 0;
        stats.Gold = 15;
        Potion = 2;
        Chance = 0.35;
    }

    public int CalculateDamage(int enemyDefence, Random Rng)
    {
        int damageDealt = Math.Max(1, (stats.Attack + 2 - (enemyDefence / 2)) + Rng.Next(0, 3));
        return Math.Max(1, damageDealt);
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