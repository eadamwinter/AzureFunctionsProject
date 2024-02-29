using System;

namespace FunctionProject.Entity.Exceptions;

internal class WrongBatteryLevelException : Exception
{
    public WrongBatteryLevelException() : base("Battery level has to be in range from 0 to 100.")
    {
    }
}
