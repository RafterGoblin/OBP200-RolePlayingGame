namespace OBP200_RolePlayingGame;

public interface IAttack
{
    int CalculateDamage(int enemyDefence, Random Rng);
}