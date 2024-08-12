using FluentValidation;
using Product;

namespace ProductService.Validators;

public class DeleteProductValidator : AbstractValidator<DeleteProductRequest>
{
    public DeleteProductValidator()
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