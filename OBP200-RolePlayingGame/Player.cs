using System.Runtime.InteropServices.ComTypes;

namespace OBP200_RolePlayingGame;

public abstract class Player : IAttack
{
    public string ClassType { get; set; }
    public int Level { get; protected set; }
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

    public abstract int CalculateDamage(int enemyDefence, Random Rng);

    public abstract int SpecialAttack(int enemyDefence, bool isBoss, Random Rng);
    public abstract void PlayerLevelUp();

    public bool IsPlayerDead()
    {
        return stats.Health <= 0;
    }
    public bool DoRest()
    {
        Console.WriteLine("Du slår läger och vilar.");
        stats.Health = stats.MaxHealth;
        Console.WriteLine("HP återställt till max.");
        return true;
    }
    
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
    public int GetExperience() => stats.Experience;
    public int GetDefence() => stats.Defence;

    public void AddPlayerExperience(int amount)
    {
        stats.Experience += amount;
    }

    public void AddPlayerGold(int amount)
    {
        stats.Gold += amount;
    }

    public void RemovePlayerGold(int amount)
    {
        stats.Gold -= amount;
    }
    public void IncreaseDefence(int amount)
    { 
        stats.Defence += amount;
    }

    public void IncreaseAttack(int amount)
    {
        stats.Attack += amount;
    }
    public int GetGold() => stats.Gold;
    
    public void ApplyDamageToPlayer(int dmg)
    {
        stats.Health -= Math.Max(0, dmg);
    }
    public void ShowStatus()
    {
        Console.WriteLine($"[{stats.Name} | {ClassType}]  HP {stats.Health}/{stats.MaxHealth}  ATK {stats.Attack}  DEF {stats.Defence}  LVL {Level}  XP {stats.Experience}  Guld {stats.Gold}  Drycker {Potion}");
        if (!string.IsNullOrWhiteSpace(Inventory))
        {
            Console.WriteLine($"Väska: {Inventory}");
        }
    }
}