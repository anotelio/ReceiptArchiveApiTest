using FluentValidation;
using ReceiptArchiveApi.Contracts.Requests;

namespace ReceiptArchiveApi.Contracts.Validators
{
    public class WrequestValidator : AbstractValidator<Wrequest>
    {
        public WrequestValidator()
        {
            RuleFor(r => r.IdRequest).NotEmpty();
            RuleFor(r => r.TitleRequest).NotEmpty();
            RuleFor(r => r.WrequestData).NotEmpty();
            RuleFor(r => r.WrequestCol).NotEmpty();
        }
    }

    public class WrequestDataValidator : AbstractValidator<WrequestData>
    {
        public WrequestDataValidator()
        {
            RuleFor(r => r.IdData).NotEmpty();
        }
    }

    public class WrequestColValidator : AbstractValidator<WrequestCol>
    {
        public WrequestColValidator()
        {
            RuleFor(r => r.IdCol).NotEmpty();
        }
    }
}
