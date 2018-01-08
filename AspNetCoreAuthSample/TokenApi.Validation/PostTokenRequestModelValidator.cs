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
                .WithMessageFormat(ApiError.MissingUsername);

            RuleFor(m => m.Password)
                .NotEmpty()
                .WithMessageFormat(ApiError.MissingPassword);

            RuleFor(m => m.Audience)
                .NotEmpty()
                .WithMessageFormat(ApiError.MissingAudience);
        }
    }
}