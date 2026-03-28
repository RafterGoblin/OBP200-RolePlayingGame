using System.Runtime.InteropServices.ComTypes;

namespace OBP200_RolePlayingGame;

public class Mage : Player
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

    public override int CalculateDamage(int enemyDefence, Random Rng)
    {
        int damageDealt = Math.Max(1, (stats.Attack + 2 - (enemyDefence / 2)) + Rng.Next(0, 3));
        return Math.Max(1, damageDealt);
    }

    public override void LevelUp()
    {
        stats.MaxHealth += 4;
        stats.Attack  += 4;
        stats.Defence += 1;
        stats.Health = stats.MaxHealth;
        Level++;
        Console.WriteLine($"Du når nivå {Level}! Värden ökade och HP återställd.");
    }

    public override int SpecialAttack(int enemyDefence, bool isBoss, Random Rng)
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
        if (isBoss)
        {
            specialDmg = (int)Math.Round(specialDmg * 0.8);
        }

        return Math.Max(0, specialDmg);
    }
}