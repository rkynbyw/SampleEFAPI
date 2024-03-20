using FluentValidation;

namespace MyRESTServices.BLL.DTOs.Validation
{
    public class ArticleCreateDTOValidator : AbstractValidator<ArticleUpdateDTO>
    {
        public ArticleCreateDTOValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title harus diisi");
        }
    }
}
