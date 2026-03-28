namespace OBP200_RolePlayingGame;

public class Rogue : Player
{
    
    
    public Rogue(string name) : base(name)
    {
        ClassType = "Rogue";
        stats.Health = 32; 
        stats.MaxHealth = 32;
        stats.Attack = 8;
        stats.Defence = 3;
        stats.Experience = 0;
        stats.Gold = 20;
        Potion = 3;
        Chance = 0.5;
    }

    public override int CalculateDamage(int enemyDefence, Random Rng)
    {
        int damageDealt = (stats.Attack - (enemyDefence / 2)) + Rng.Next(0, 3);
        damageDealt += (Rng.NextDouble() < 0.2) ? 4 : 0;
        return Math.Max(1, damageDealt);
    }
    
    public override void LevelUp()
    {
        stats.MaxHealth += 5;
        stats.Attack  += 3;
        stats.Defence += 1;
        stats.Health = stats.MaxHealth;
        Level++;
        Console.WriteLine($"Du når nivå {Level}! Värden ökade och HP återställd.");
    }
    
    public override int SpecialAttack(int enemyDefence, bool isBoss, Random Rng)
    {
        int specialDmg;
        if (Rng.NextDouble() < 0.5)
        {
            Console.WriteLine("Rogue utför en lyckad Backstab!");
            specialDmg = Math.Max(4, stats.Attack + 6);
        }
        else
        {
            Console.WriteLine("Backstab misslyckades!");
            specialDmg = 1;
        }
        if (isBoss)
        {
            specialDmg = (int)Math.Round(specialDmg * 0.8);
        }

        return Math.Max(0, specialDmg);
    }
}