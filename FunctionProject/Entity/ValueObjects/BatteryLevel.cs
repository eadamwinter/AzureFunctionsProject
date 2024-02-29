using FunctionProject.Entity.Exceptions;

namespace FunctionProject.Entity.ValueObjects;

internal sealed record BatteryLevel
{
    public BatteryLevel(int level)
    {
        Validate(level);
        Value = level;
    }

    internal int Value { get; init; }

    private void Validate(int level)
    {
        if (level < 0 || level > 100)
        {
            throw new WrongBatteryLevelException();
        }
    }
}
