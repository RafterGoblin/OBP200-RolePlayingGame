namespace OBP200_RolePlayingGame;

public class Rogue : Player, IAttack
{
    
    
    public Rogue(string name) : base(name)
    {
        stats.Health = 32; 
        stats.MaxHealth = 32;
        stats.Attack = 8;
        stats.Defence = 3;
        stats.Experience = 0;
        stats.Gold = 20;
        Potion = 3;
        Chance = 0.5;
    }

    public int CalculateDamage(int enemyDefence, Random Rng)
    {
        int damageDealt = (stats.Attack - (enemyDefence / 2)) + Rng.Next(0, 3);
        damageDealt += (Rng.NextDouble() < 0.2) ? 4 : 0;
        return Math.Max(1, damageDealt);
    }
    public override int SpecialAttack(int attack, int enemyDefence, Random Rng)
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
        return specialDmg;
    }
}