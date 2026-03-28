using System.Text;

namespace OBP200_RolePlayingGame;


class Program
{
    // ======= Globalt tillstånd  =======
    
    private static List<Enemy> enemies = new List<Enemy>();
    private static Player player;
    // Rum: [type, label]
    // types: battle, treasure, shop, rest, boss
    static List<string[]> Rooms = new List<string[]>();

    // Status för kartan
    static int CurrentRoomIndex = 0;

    // Random
    static Random Rng = new Random();

    // ======= Main =======

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        InitEnemyTemplates();

        while (true)
        {
            ShowMainMenu();
            Console.Write("Välj: ");
            var choice = (Console.ReadLine() ?? "").Trim();

            if (choice == "1")
            {
                StartNewGame();
                RunGameLoop();
            }
            else if (choice == "2")
            {
                Console.WriteLine("Avslutar...");
                return;
            }
            else
            {
                Console.WriteLine("Ogiltigt val.");
            }

            Console.WriteLine();
        }
    }

    // ======= Meny & Init =======

    static void ShowMainMenu()
    {
        Console.WriteLine("=== Text-RPG ===");
        Console.WriteLine("1. Nytt spel");
        Console.WriteLine("2. Avsluta");
    }

    static void StartNewGame()
    {
        Console.Write("Ange namn: ");
        var name = (Console.ReadLine() ?? "").Trim();
        if (string.IsNullOrWhiteSpace(name)) name = "Namnlös";

        Console.WriteLine("Välj klass: 1) Warrior  2) Mage  3) Rogue");
        Console.Write("Val: ");
        var k = (Console.ReadLine() ?? "").Trim();

        string cls = player.ClassType;
        
        switch (k)
        {
            case "1": // Warrior: tankig
                player = new Warrior(name);
                break;
            case "2": // Mage: hög damage, låg def
                player = new Mage(name);
                break;
            case "3": // Rogue: krit-chans
                player = new Rogue(name);
                break;
            default:
                player  = new Warrior(name);
                break;
        }

        // Initiera karta (linjärt äventyr)
        Rooms.Clear();
        Rooms.Add(new[] { "battle", "Skogsstig" });
        Rooms.Add(new[] { "treasure", "Gammal kista" });
        Rooms.Add(new[] { "shop", "Vandrande köpman" });
        Rooms.Add(new[] { "battle", "Grottans mynning" });
        Rooms.Add(new[] { "rest", "Lägereld" });
        Rooms.Add(new[] { "battle", "Grottans djup" });
        Rooms.Add(new[] { "boss", "Urdraken" });

        CurrentRoomIndex = 0;

        Console.WriteLine($"Välkommen, {name} the {cls}!");
        player.ShowStatus();
    }

    static void RunGameLoop()
    {
        while (true)
        {
            var room = Rooms[CurrentRoomIndex];
            Console.WriteLine($"--- Rum {CurrentRoomIndex + 1}/{Rooms.Count}: {room[1]} ({room[0]}) ---");

            bool continueAdventure = EnterRoom(room[0]);
            
            if (player.IsPlayerDead())
            {
                Console.WriteLine("Du har stupat... Spelet över.");
                break;
            }
            
            if (!continueAdventure)
            {
                Console.WriteLine("Du lämnar äventyret för nu.");
                break;
            }

            CurrentRoomIndex++;
            
            if (CurrentRoomIndex >= Rooms.Count)
            {
                Console.WriteLine();
                Console.WriteLine("Du har klarat äventyret!");
                break;
            }
            
            Console.WriteLine();
            Console.WriteLine("[C] Fortsätt     [Q] Avsluta till huvudmeny");
            Console.Write("Val: ");
            var post = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

            if (post == "Q")
            {
                Console.WriteLine("Tillbaka till huvudmenyn.");
                break;
            }

            Console.WriteLine();
        }
    }

    // ======= Rumshantering =======

    static bool EnterRoom(string type)
    {
        switch ((type ?? "battle").Trim())
        {
            case "battle":
                return DoBattle(isBoss: false);
            case "boss":
                return DoBattle(isBoss: true);
            case "treasure":
                return DoTreasure();
            case "shop":
                return DoShop();
            case "rest":
                return player.DoRest();
            default:
                Console.WriteLine("Du vandrar vidare...");
                return true;
        }
    }

    // ======= Strid =======

    static bool DoBattle(bool isBoss)
    {
        var enemy = GenerateEnemy(isBoss);
        enemy.EnemyAppeared();

        while (enemy.GetHealth() > 0 && !player.IsPlayerDead())
        {
            Console.WriteLine();
            player.ShowStatus();
            Console.WriteLine($"Fiende: {enemy.GetName()} HP={enemy.GetHealth()}");
            Console.WriteLine("[A] Attack   [X] Special   [P] Dryck   [R] Fly");
            if (isBoss) Console.WriteLine("(Du kan inte fly från en boss!)");
            Console.Write("Val: ");

            var cmd = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

            if (cmd == "A")
            {
                int damage = player.CalculateDamage(enemy.GetDefence(), Rng);
                enemy.TakeDamage(damage);
                Console.WriteLine($"Du slog {enemy.GetName()} för {damage} skada.");
            }
            else if (cmd == "X")
            {
                int special = player.SpecialAttack(enemy.GetDefence(), isBoss, Rng);
                enemy.TakeDamage(special);
                Console.WriteLine($"Special! {enemy.GetName()} tar {special} skada.");
            }
            else if (cmd == "P")
            {
                player.UsePotion();
            }
            else if (cmd == "R" && !isBoss)
            {
                if (player.TryRunAway(Rng))
                {
                    Console.WriteLine("Du flydde!");
                    return true; // fortsätt äventyr
                }
                else
                {
                    Console.WriteLine("Misslyckad flykt!");
                }
            }
            else
            {
                Console.WriteLine("Du tvekar...");
            }

            if (enemy.GetHealth() <= 0)
            {
                enemy.ResetHealth();
                break;
            }
                

            // Fiendens tur
            int enemyDamage = enemy.CalculateDamage(player.GetDefence(), Rng);
            player.ApplyDamageToPlayer(enemyDamage);
            Console.WriteLine($"{enemy.GetName()} anfaller och gör {enemyDamage} skada!");
        }

        if (player.IsPlayerDead())
        {
            return false; // avsluta äventyr
        }

        // Vinstrapporter, XP, guld, loot
        AddPlayerXp(enemy.GetExperience());
        AddPlayerGold(enemy.GetGold());

        Console.WriteLine($"Seger! +{enemy.GetExperience()} XP, +{enemy.GetGold()} guld.");
        MaybeDropLoot(enemy.GetName());

        return true;
    }

    static Enemy GenerateEnemy(bool isBoss)
    {
        if (isBoss)
        {
            // Boss-mall
            return new Enemy("boss", "Urdraken", 55, 9, 4, 30, 50, Rng );
        }
        else
        {
            // Slumpa bland templates
            var enemy = enemies[Rng.Next(enemies.Count)];
            return enemy;
        }
    }

    static void InitEnemyTemplates()
    {
        enemies.Clear();
        enemies.Add(new Enemy("beast", "Vildsvin", 18, 4, 1, 6, 4, Rng ));
        enemies.Add(new Enemy("undead", "Skelett", 20, 5, 2, 7, 5, Rng ));
        enemies.Add(new Enemy("bandit", "Bandit", 16, 6, 1, 8, 6, Rng ));
        enemies.Add(new Enemy("slime", "Geléslem", 14, 3, 0, 5, 3, Rng ));
    }
    
    static void AddPlayerXp(int amount)
    {
        player.AddPlayerExperience(amount);
        MaybeLevelUp();
    }

    static void AddPlayerGold(int amount)
    {
        player.AddPlayerGold(amount);
    }

    static void MaybeLevelUp()
    {
        // Nivåtrösklar
        int nextThreshold = player.Level == 1 ? 10 : (player.Level == 2 ? 25 : (player.Level == 3 ? 45 : player.Level * 20));

        if (player.GetExperience() >= nextThreshold)
        {
            player.PlayerLevelUp();
        }
    }

    static void MaybeDropLoot(string enemyName)
    {
        // Enkel loot-regel
        if (Rng.NextDouble() < 0.35)
        {
            string item = "Minor Gem";
            if (enemyName.Contains("Urdraken")) item = "Dragon Scale";

            var inv = (player.Inventory ?? "").Trim();
            if (string.IsNullOrEmpty(inv)) player.Inventory = item;
            else player.Inventory = inv + ";" + item;

            Console.WriteLine($"Föremål hittat: {item} (lagt i din väska)");
        }
    }

    // ======= Rumshändelser =======

    static bool DoTreasure()
    {
        Console.WriteLine("Du hittar en gammal kista...");
        if (Rng.NextDouble() < 0.5)
        {
            int gold = Rng.Next(8, 15);
            player.AddPlayerGold(gold);
            Console.WriteLine($"Kistan innehåller {gold} guld!");
        }
        else
        {
            var items = new[] { "Iron Dagger", "Oak Staff", "Leather Vest", "Healing Herb" };
            string found = items[Rng.Next(items.Length)];
            var inv = (player.Inventory ?? "").Trim();
            player.Inventory = string.IsNullOrEmpty(inv) ? found : (inv + ";" + found);
            Console.WriteLine($"Du plockar upp: {found}");
        }
        return true;
    }

    static bool DoShop()
    {
        Console.WriteLine("En vandrande köpman erbjuder sina varor:");
        while (true)
        {
            Console.WriteLine($"Guld: {player.GetGold()} | Drycker: {player.Potion}");
            Console.WriteLine("1) Köp dryck (10 guld)");
            Console.WriteLine("2) Köp vapen (+2 ATK) (25 guld)");
            Console.WriteLine("3) Köp rustning (+2 DEF) (25 guld)");
            Console.WriteLine("4) Sälj alla 'Minor Gem' (+5 guld/st)");
            Console.WriteLine("5) Lämna butiken");
            Console.Write("Val: ");
            var val = (Console.ReadLine() ?? "").Trim();

            if (val == "1")
            {
                TryBuy(10, () => player.Potion += 1, "Du köper en dryck.");
            }
            else if (val == "2")
            {
                TryBuy(25, () => player.IncreaseAttack(2), "Du köper ett bättre vapen.");
            }
            else if (val == "3")
            {
                TryBuy(25, () => player.IncreaseDefence(2), "Du köper bättre rustning.");
            }
            else if (val == "4")
            {
                SellMinorGems();
            }
            else if (val == "5")
            {
                Console.WriteLine("Du säger adjö till köpmannen.");
                break;
            }
            else
            {
                Console.WriteLine("Köpmannen förstår inte ditt val.");
            }
        }
        return true;
    }

    static void TryBuy(int cost, Action apply, string successMsg)
    {
        int gold = player.GetGold();
        if (gold >= cost)
        {
            player.RemovePlayerGold(cost);
            apply();
            Console.WriteLine(successMsg);
        }
        else
        {
            Console.WriteLine("Du har inte råd.");
        }
    }

    static void SellMinorGems()
    {
        var inv = (player.Inventory ?? "");
        if (string.IsNullOrWhiteSpace(inv))
        {
            Console.WriteLine("Du har inga föremål att sälja.");
            return;
        }

        var items = inv.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
        int count = items.Count(x => x == "Minor Gem");
        if (count == 0)
        {
            Console.WriteLine("Inga 'Minor Gem' i väskan.");
            return;
        }

        items = items.Where(x => x != "Minor Gem").ToList();
        player.Inventory = items.Count == 0 ? "" : string.Join(";", items);

        AddPlayerGold(count * 5);
        Console.WriteLine($"Du säljer {count} st Minor Gem för {count * 5} guld.");
    }

}
