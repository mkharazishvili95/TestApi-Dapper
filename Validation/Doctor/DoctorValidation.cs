using FluentValidation;
using TestApi.UseCases.Doctor.Create;

namespace TestApi.Validation.Doctor
{
    public class DoctorValidation : AbstractValidator<CreateDoctorRequest>
    {
        public DoctorValidation()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.")
                .Length(1,20).WithMessage("First name must be from 1 to 20");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.")
                .Length(1, 20).WithMessage("Last name must be from 1 to 20");
        }
    }
}
