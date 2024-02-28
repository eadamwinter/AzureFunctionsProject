using MediatR;

namespace FunctionProject.myservice
{
    public sealed record ValidateMessageCommand(string message) : IRequest<bool>;
}
