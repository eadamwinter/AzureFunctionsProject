using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionProject.myservice
{
    public sealed class MessageValidator : IMessageValidator
    {
        public string Validate(string message)
        {
            try
            {
                var cos = JsonConvert.DeserializeObject<Plik>(message);
            }
            catch (Exception ex)
            {
                return $"{ex.Message}";
            }

            return string.Empty;
        }
    }
}
