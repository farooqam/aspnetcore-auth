using FluentValidation;
using TokenApi.Common;

namespace TokenApi.Validation
{
    public class PostTokenRequestModelValidator : AbstractValidator<PostTokenRequestModel>
    {
        public PostTokenRequestModelValidator()
        {
            RuleFor(m => m.Username)
                .NotEmpty()
                .WithErrorCode(ApiError.MissingUsername.Code.ToString())
                .WithMessage(ApiError.MissingUsername.Message);

            RuleFor(m => m.Password)
                .NotEmpty()
                .WithErrorCode(ApiError.MissingPassword.Code.ToString())
                .WithMessage(ApiError.MissingPassword.Message);

            RuleFor(m => m.Audience)
                .NotEmpty()
                .WithErrorCode(ApiError.MissingAudience.Code.ToString())
                .WithMessage(ApiError.MissingAudience.Message);
        }
    }
}