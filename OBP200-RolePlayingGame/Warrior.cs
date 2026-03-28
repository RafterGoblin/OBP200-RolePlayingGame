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
        Potion = 2;
        Chance = 0.25;
    }
    
    public override int CalculateDamage(int enemyDefence, Random Rng)
    {
        int damageDealt = Math.Max(1, (stats.Attack + 1 - (enemyDefence / 2)) + Rng.Next(0, 3));
        return Math.Max(1, damageDealt);
    }
    
    public override void LevelUp()
    {
        stats.MaxHealth += 6;
        stats.Attack  += 2;
        stats.Defence += 2;
        stats.Health = stats.MaxHealth;
        Level++;
        Console.WriteLine($"Du når nivå {Level}! Värden ökade och HP återställd.");
    }
    
    public override int SpecialAttack(int enemyDefence, bool isBoss, Random Rng)
    {
        int specialDmg = 0;
        Console.WriteLine("Warrior använder Heavy Strike!");
        specialDmg = Math.Max(2, stats.Attack + 3 - enemyDefence);
        stats.Health -= 2;
        // Dämpa skada mot bossen
        if (isBoss)
        {
            specialDmg = (int)Math.Round(specialDmg * 0.8);
        }

        return Math.Max(0, specialDmg);
    }
}