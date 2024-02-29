using System;

namespace FunctionProject.Entity.Exceptions;

internal class WrongDesignationException : Exception
{
    public WrongDesignationException() : base("Designation must have length of 6.")
    {
    }
}
