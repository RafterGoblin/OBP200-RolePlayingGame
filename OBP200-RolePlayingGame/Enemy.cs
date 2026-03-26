using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;

namespace OBP200_RolePlayingGame;

public class Enemy
{
    public Character stats =  new Character();
    
    public Enemy(string race, string name, int health, int attack, int defence, int experience, int gold, Random Rng)
    {
       stats.Name = name; 
       stats.Health = health + Rng.Next(-1, 3);
       stats.Attack = attack +  Rng.Next(0, 2);
       stats.Defence = defence + Rng.Next(0, 2);
       stats.Experience = experience +  Rng.Next(0, 3);
       stats.Gold = gold + Rng.Next(0, 3);
    }
    
    
   /* static void InitEnemyTemplates()
    {
        EnemyTemplates.Clear();
        EnemyTemplates.Add(new[] { "beast", "Vildsvin", "18", "4", "1", "6", "4" });
        EnemyTemplates.Add(new[] { "undead", "Skelett", "20", "5", "2", "7", "5" });
        EnemyTemplates.Add(new[] { "bandit", "Bandit", "16", "6", "1", "8", "6" });
        EnemyTemplates.Add(new[] { "slime", "Geléslem", "14", "3", "0", "5", "3" });
    }
    */
}