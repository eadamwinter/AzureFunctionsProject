using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionProject.Services;

public sealed class ValidateMessageCommandHandler : IRequestHandler<ValidateMessageCommand, ResultDto>
{
    public ValidateMessageCommandHandler(IMessageValidator messageValidator)
    {
        MessageValidator = messageValidator;
    }

    public IMessageValidator MessageValidator { get; }

    public async Task<ResultDto> Handle(ValidateMessageCommand request, CancellationToken cancellationToken)
    {
        var result = MessageValidator.Validate(request.message);

        if (result == string.Empty)
        {
            return new ResultDto(true, result);
        }

        return new ResultDto(false, result);
    }
}
