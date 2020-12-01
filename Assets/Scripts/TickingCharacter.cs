//Character that has a number of ticks left before an effect ends
public class TickingCharacter
{
    public CombatController Enemy;
    public int Ticks;

    public TickingCharacter(CombatController enemy, int ticks)
    {
        Enemy = enemy;
        Ticks = ticks;
    }
}