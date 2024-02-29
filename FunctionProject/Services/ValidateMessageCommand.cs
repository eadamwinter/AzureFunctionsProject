using MediatR;

namespace FunctionProject.Services;

public sealed record ValidateMessageCommand(string message) : IRequest<ResultDto>;
