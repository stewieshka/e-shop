using FluentValidation;
using Product;

namespace ProductService.Validators;

public class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();
        
        RuleFor(x => x.Description)
            .NotNull()
            .NotEmpty();
        
        RuleFor(x => x.Category)
            .NotNull()
            .NotEmpty();
        
        RuleFor(x => x.Price)
            .NotNull()
            .NotEmpty();
        
        RuleFor(x => x.Stock)
            .NotNull()
            .NotEmpty();
    }
}