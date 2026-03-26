using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;

namespace OBP200_RolePlayingGame;

public class Enemy : IAttack
{
    public Character stats =  new Character();
    public string Race { get;  set; }
    
    
    public Enemy(string race, string name, int health, int attack, int defence, int experience, int gold, Random Rng)
    { 
        Race = race;
        stats.Name = name; 
        stats.Health = health + Rng.Next(-1, 3);
        stats.Attack = attack +  Rng.Next(0, 2);
        stats.Defence = defence + Rng.Next(0, 2);
        stats.Experience = experience +  Rng.Next(0, 3);
        stats.Gold = gold + Rng.Next(0, 3);
    }
    
    public int CalculateDamage(int enemyDefence, Random Rng)
    {
        int damageDealt = Math.Max(1, stats.Attack - (enemyDefence / 2)) + Rng.Next(0, 3);

        // Liten chans till "glancing blow" (minskad skada)
        if (Rng.NextDouble() < 0.1) damageDealt = Math.Max(1, damageDealt - 2);

        return damageDealt;
    }
}