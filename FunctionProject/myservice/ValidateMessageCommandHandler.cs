using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionProject.myservice
{
    public sealed class ValidateMessageCommandHandler : IRequestHandler<ValidateMessageCommand, bool>
    {
        public ValidateMessageCommandHandler(IMessageValidator messageValidator)
        {
            MessageValidator = messageValidator;
        }

        public IMessageValidator MessageValidator { get; }

        public async Task<bool> Handle(ValidateMessageCommand request, CancellationToken cancellationToken)
        {
            var result = MessageValidator.Validate(request.message);

            if(result == string.Empty)
            {
                return true;
            }
            
            return false;
        }
    }
}
