using System;

class Game1
{
    static Random r = new Random();

    static string[][] planets =
    {
        new[] { "Moon", "Mars", "Nebula", "Titan" },
        new[] { "Ruins", "Black Hole", "Ice World", "Golden Planet" },
        new[] { "Europa", "Asteroid Belt", "Solar Station", "Deep Space" }
    };

    static string[] enemies =
    {
        "Raider","Drone","Pirate","Beast","Hunter","Mutant",
        "War Machine","Void Stalker","Alien","Space Worm",
        "Dark Knight","Star Beast"
    };

    static int[] eDmg = { 15,20,25,30,35,22,28,32,18,27,34,40 };

    static string[] bosses =
    {
        "Galaxy Destroyer","Void Reaper","Titan Overlord","Earth's Destroyer"
    };

    static int[] bHP = { 150,250,350,600 };
    static int[] bDmg = { 25,40,50,75 };

    static string[] weapons = { "Laser","Missile","Plasma","Nova" };
    static int[] dmg = { 20,30,40,50 };
    static int[] ammo = { 10,4,0,0 };
    static int[] maxAmmo = { 20,10,5,2 };

    static int hp, fuel, credits, score, heals, power;
    static int phase, encounter, enemyHP, enemyDmg; 
    static bool boss, earth;
    static string enemy;
    static bool[] used = new bool[12];

    static void Main()
    {
        while (true)
        {
            NewGame();

            Console.Write("Captain: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) name = "Explorer";

            while (hp > 0 && fuel > 0 && !earth)
            {
                Console.WriteLine(
                    $"\n{name} | HP:{hp} | Fuel:{fuel} | Credits:{credits}");
                Console.WriteLine(
                    $"Phase:{Math.Min(phase + 1, 3)}/3 | {encounter}/4 | Score:{score}");
                Console.WriteLine(
                    $"Ammo L:{ammo[0]}/{maxAmmo[0]} M:{ammo[1]}/{maxAmmo[1]} " +
                    $"P:{ammo[2]}/{maxAmmo[2]} N:{ammo[3]}/{maxAmmo[3]}");
                Console.WriteLine($"Heal:{heals} | Power:{power}");

                Console.Write(
                    "\n[1] Travel [2] Stats [3] Shop [4] Achievements [5] Exit\n> ");

                switch (Console.ReadLine())
                {
                    case "1": Travel(); break;
                    case "2": Stats(name); break;
                    case "3": Shop(); break;
                    case "4": Achievements(); break;
                    case "5": return;
                    default: Console.WriteLine("Invalid option!"); break;
                }
            }

            if (earth)
                Console.WriteLine($"\n*** EARTH SAVED! ***\nFinal Score: {score}");
            else
                Console.WriteLine(hp <= 0 ? "\nSHIP DESTROYED!" : "\nOUT OF FUEL!");

            Console.Write("\nPlay again? Y/N: ");
            if (Console.ReadLine()?.ToUpper() != "Y") return;
        }
    }

    static void NewGame()
    {
        hp = 100;
        fuel = 5;
        credits = 50;
        score = heals = power = phase = encounter = 0;

        boss = earth = false;

        dmg = new[] { 20,30,40,50 };
        ammo = new[] { 10,4,0,0 };
        used = new bool[12];

        Console.Clear();
        Console.WriteLine("================================");
        Console.WriteLine("         SPACE EXPLORER");
        Console.WriteLine("================================");
    }

    static void Travel()
    {
        fuel--;

        Console.WriteLine($"\n--- PHASE {phase + 1} | {encounter + 1}/4 ---");

        if (r.Next(2) == 0)
        {
            Console.WriteLine($">> {planets[phase][encounter]}");
            Reward();
        }
        else
            StartEnemy();

        score += 10;
        encounter++;

        if (encounter == 4)
        {
            encounter = 0;
            StartBoss();
        }
    }

    static void StartEnemy()
    {
        int i;

        do
            i = r.Next(enemies.Length);
        while (used[i]);

        used[i] = true;
        enemy = enemies[i];
        enemyDmg = eDmg[i];
        enemyHP = r.Next(30, 61);

        Console.WriteLine($"\n!!! {enemy} APPEARED !!!");

        if (Battle())
        {
            score += 50;
            credits += 25;
            Console.WriteLine("\n[REWARD] +25 Credits +50 Score");
        }
    }

    static void StartBoss()
    {
        enemy = bosses[phase];
        enemyHP = bHP[phase];
        enemyDmg = bDmg[phase];
        boss = true;

        Console.WriteLine($"\n!!! BOSS: {enemy} !!!");
        Console.WriteLine($"HP:{enemyHP} | Damage:{enemyDmg}");

        if (!Battle())
        {
            boss = false;
            return;
        }

        score += 200;
        credits += 200;
        power++;

        Console.WriteLine("\n[BOSS REWARD] +200 Credits +200 Score");
        Console.WriteLine("[BOSS] +1 Powerup");

        boss = false;
        phase++;

        if (phase < 3)
            Console.WriteLine($"\n*** PHASE {phase + 1} ***");
        else
            FinalBoss();
    }

    static void FinalBoss()
    {
        boss = true;
        enemy = bosses[3];
        enemyHP = bHP[3];
        enemyDmg = bDmg[3];

        Console.WriteLine("\n================================");
        Console.WriteLine("             EARTH");
        Console.WriteLine("================================");
        Console.WriteLine($"!!! FINAL BOSS: {enemy} !!!");
        Console.WriteLine($"HP:{enemyHP} | Damage:{enemyDmg}");

        if (Battle())
        {
            score += 200;
            credits += 200;
            boss = false;
            earth = true;

            Console.WriteLine("\n*** FINAL BOSS DEFEATED! ***");
            Console.WriteLine("*** EARTH SAVED! ***");
        }
        else
            boss = false;
    }

    static bool Battle()
    {
        while (enemyHP > 0 && hp > 0)
        {
            Console.WriteLine($"\n{enemy} HP:{enemyHP} | YOU:{hp}");
            Console.WriteLine(
                $"L:{ammo[0]} M:{ammo[1]} P:{ammo[2]} N:{ammo[3]}");
            Console.Write("[1] Attack [2] Bag [3] Run\n> ");

            switch (Console.ReadLine())
            {
                case "1":
                    Attack();
                    break;

                case "2":
                    Bag();
                    break;

                case "3":
                    if (boss)
                        Console.WriteLine("You cannot run!");
                    else
                    {
                        Console.WriteLine("Escaped!");
                        return false;
                    }
                    break;

                default:
                    Console.WriteLine("Invalid option!");
                    break;
            }
        }

        if (hp <= 0) return false;

        Console.WriteLine($"\n*** {enemy} DEFEATED! ***");
        return true;
    }

    static void Attack()
    {
        for (int i = 0; i < 4; i++)
            Console.WriteLine(
                $"{i + 1}. {weapons[i]} {dmg[i]} [{ammo[i]}/{maxAmmo[i]}]");

        Console.Write("> ");

        if (!int.TryParse(Console.ReadLine(), out int a) ||
            a < 1 || a > 4)
        {
            Console.WriteLine("Invalid weapon!");
            return;
        }

        int i2 = a - 1;

        if (ammo[i2] <= 0)
        {
            Console.WriteLine("[EMPTY]");
            return;
        }

        ammo[i2]--;

        int hit = Math.Max(1, dmg[i2] + r.Next(-5, 6));
        enemyHP -= hit;

        Console.WriteLine($"[ATTACK] {weapons[i2]} -{hit} HP");

        if (enemyHP > 0)
            Hit();
    }

    static void Bag()
    {
        Console.Write($"[1] Heal({heals}) [2] Power({power})\n> ");

        switch (Console.ReadLine())
        {
            case "1":
                if (heals <= 0)
                {
                    Console.WriteLine("No heals!");
                    return;
                }

                int old = hp;
                hp = Math.Min(100, hp + r.Next(20, 41));
                heals--;

                Console.WriteLine($"[HEAL] +{hp - old}");

                if (enemyHP > 0)
                    Hit();
                break;

            case "2":
                if (power <= 0)
                {
                    Console.WriteLine("No powerups!");
                    return;
                }

                power--;

                for (int i = 0; i < 4; i++)
                    dmg[i] += 10;

                Console.WriteLine("[POWER] All damage +10");

                if (enemyHP > 0)
                    Hit();
                break;

            default:
                Console.WriteLine("Invalid option!");
                break;
        }
    }

    static void Hit()
    {
        if (enemyHP <= 0) return;

        int hit = Math.Max(1, enemyDmg + r.Next(-5, 6));
        hp -= hit;

        Console.WriteLine($"[ENEMY] -{hit} HP");
    }

    static void Reward()
    {
        switch (r.Next(3))
        {
            case 0:
                fuel += 2;
                Console.WriteLine("[REWARD] +2 Fuel");
                break;

            case 1:
                credits += 30;
                Console.WriteLine("[REWARD] +30 Credits");
                break;

            case 2:
                heals++;
                Console.WriteLine("[REWARD] +1 Heal");
                break;
        }
    }

    static void Shop()
    {
        Console.WriteLine($"\n========== SHOP ==========");
        Console.WriteLine($"Credits: {credits}");
        Console.WriteLine("1 Heal +1       20C");
        Console.WriteLine("2 Fuel +2       15C");
        Console.WriteLine("3 Laser +5      10C");
        Console.WriteLine("4 Missile +3    15C");
        Console.WriteLine("5 Plasma +2     20C");
        Console.WriteLine("6 Nova +1       30C");
        Console.WriteLine("7-10 Weapon Damage +10");

        Console.Write("> ");

        if (!int.TryParse(Console.ReadLine(), out int n))
        {
            Console.WriteLine("Invalid option!");
            return;
        }

        if (n == 1)
        {
            if (credits < 20)
            {
                Console.WriteLine("Not enough credits!");
                return;
            }

            credits -= 20;
            heals++;
        }
        else if (n == 2)
        {
            if (credits < 15)
            {
                Console.WriteLine("Not enough credits!");
                return;
            }

            credits -= 15;
            fuel += 2;
        }
        else if (n >= 3 && n <= 6)
        {
            int i = n - 3;
            int[] amount = { 5,3,2,1 };
            int[] cost = { 10,15,20,30 };

            if (ammo[i] >= maxAmmo[i])
            {
                Console.WriteLine("Ammo already full!");
                return;
            }

            if (credits < cost[i])
            {
                Console.WriteLine("Not enough credits!");
                return;
            }

            credits -= cost[i];
            ammo[i] = Math.Min(maxAmmo[i], ammo[i] + amount[i]);
        }
        else if (n >= 7 && n <= 10)
        {
            int i = n - 7;
            int cost = 40 + i * 10;

            if (credits < cost)
            {
                Console.WriteLine("Not enough credits!");
                return;
            }

            credits -= cost;
            dmg[i] += 10;
        }
        else
            Console.WriteLine("Invalid option!");
    }

    static void Stats(string name)
    {
        Console.WriteLine($"\nCaptain: {name}");
        Console.WriteLine(
            $"Phase:{Math.Min(phase + 1, 3)}/3 | Encounter:{encounter}/4");
        Console.WriteLine($"HP:{hp} Fuel:{fuel} Credits:{credits}");
        Console.WriteLine($"Score:{score} Heals:{heals} Power:{power}");

        for (int i = 0; i < 4; i++)
            Console.WriteLine(
                $"{weapons[i]}: {dmg[i]} Damage | {ammo[i]}/{maxAmmo[i]} Ammo");
    }

    static void Achievements()
    {
        Console.WriteLine("\n========== ACHIEVEMENTS ==========");
        Console.WriteLine($"Phase: {Math.Min(phase + 1, 3)}/3");
        Console.WriteLine($"Bosses Defeated: {Math.Min(phase, 3)}/3");
        Console.WriteLine($"Score: {score}");
        Console.WriteLine($"Powerups: {power}");
        Console.WriteLine($"Earth: {earth}");
    }
}
