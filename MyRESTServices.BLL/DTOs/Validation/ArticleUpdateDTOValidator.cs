using FluentValidation;

namespace MyRESTServices.BLL.DTOs.Validation
{
    public class ArticleUpdateDTOValidator : AbstractValidator<ArticleCreateDTO>
    {
        public ArticleUpdateDTOValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title harus diisi");
        }

    }
}
