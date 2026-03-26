using System.Runtime.InteropServices.ComTypes;

namespace OBP200_RolePlayingGame;

public abstract class Player
{
    public string ClassType { get; set; }
    public int Level { get; set; }
    public int Potion { get; set; }
    public string Inventory { get; set; }
    public Character stats =  new Character();
    public double Chance { get; set; }

    public Player(string name)
    {
        stats.Name = name;
        stats.Experience = 0;  
        ClassType = "Player";
        Level = 1;
        Inventory = "Wooden Sword;Cloth Armor";
    }

    
    public abstract int SpecialAttack(int attack, int enemyDefence, Random Rng);

    public bool TryRunAway(Random Rng)
    {
        return Rng.NextDouble() < Chance;
    }
    public void UsePotion()
    {
        if (Potion <= 0)
        {
            Console.WriteLine("Du har inga drycker kvar.");
        }
        // Helning av spelaren
        else
        {
            int heal = 12;
            int newHp = Math.Min(stats.MaxHealth, stats.Health + heal);
            Console.WriteLine($"Du dricker en dryck och återfår {newHp - stats.Health} HP.");
            stats.Health = newHp;
            Potion -= 1;
        }
    }
    
    public void ApplyDamageToPlayer(int dmg)
    {
        stats.Health -= Math.Max(0, dmg);
    }
}