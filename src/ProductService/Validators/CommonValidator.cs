using System.Text;
using FluentValidation;
using Grpc.Core;

namespace ProductService.Validators;

public class CommonValidator<TRequest, TValidator>
    where TValidator : AbstractValidator<TRequest>, new()
{
    public async Task ValidateOrThrowAsync(TRequest request)
    {
        var validator = new TValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errorDetails = new StringBuilder();
            foreach (var error in validationResult.Errors)
            {
                errorDetails.Append($"{error.ErrorMessage} |");
            }

            throw new RpcException(new Status(StatusCode.InvalidArgument, errorDetails.ToString()));
        }
    }
}