using FluentValidation;
using TestApi.UseCases.Person.Create;

namespace TestApi.Validation.Person
{
    public class PersonValidation : AbstractValidator<CreatePersonRequest>
    {
        public PersonValidation() 
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.")
                .Length(1,20).WithMessage("First name must be from 1 to 20");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.")
                .Length(1, 20).WithMessage("Last name must be from 1 to 20");

            RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Date of birth is required.");
        }
    }
}
