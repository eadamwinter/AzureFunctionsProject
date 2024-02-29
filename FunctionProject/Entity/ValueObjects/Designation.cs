using FunctionProject.Entity.Exceptions;

namespace FunctionProject.Entity.ValueObjects;

internal sealed record Designation
{
    public Designation(string value)
    {
        Validate(value);
        Value = value;
    }

    internal string Value { get; init; }

    private void Validate(string designation)
    {
        if (designation.Length != 6)
        {
            throw new WrongDesignationException();
        }
    }
}
