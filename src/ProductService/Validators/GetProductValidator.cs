using FluentValidation;
using Product;

namespace ProductService.Validators;

public class GetProductValidator : AbstractValidator<GetProductRequest>
{
    public GetProductValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotNull()
            .Must(BeGuid);
    }

    private bool BeGuid(string x)
    {
        return Guid.TryParse(x, out _);
    }
}