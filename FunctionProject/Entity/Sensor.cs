using FunctionProject.Entity.Enums;

namespace FunctionProject.Entity;

internal sealed record Sensor(string designation, Location location, int batteryLevel);