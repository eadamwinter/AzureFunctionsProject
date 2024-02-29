using FunctionProject.Entity;
using FunctionProject.Entity.ValueObjects;
using Newtonsoft.Json;
using System;

namespace FunctionProject.Services;

public sealed class MessageValidator : IMessageValidator
{
    public string Validate(string message)
    {
        try
        {
            var sensor = JsonConvert.DeserializeObject<Sensor>(message);

            _ = new Designation(sensor.designation);
            _ = new BatteryLevel(sensor.batteryLevel);
        }
        catch (Exception ex)
        {
            return $"{ex.Message}";
        }

        return string.Empty;
    }
}
