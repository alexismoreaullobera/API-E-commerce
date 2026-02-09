using EcommerceAPI.Application.DTOs;
using FluentValidation;

namespace EcommerceAPI.Application.Validators;

public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Le nom est obligatoire")
            .MaximumLength(200).WithMessage("Le nom ne peut dépasser 200 caractères");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La description ne peut être vide")
            .MaximumLength(1000).WithMessage("La description ne peut dépasser 1000 caractères");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Le prix doit être supérieur à 0");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Le stock ne peut être négatif");

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("L'URL de l'image est obligatoire");
    }
}