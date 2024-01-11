using System;
using FluentValidation;
using Paperless.BusinessLogic.Entities;

namespace Paperless.BusinessLogic
{
	public class DocumentTypeValidator : AbstractValidator<DocumentType>
	{
		public DocumentTypeValidator()
		{
            // nochmal checken welche properties alle required sind..., hier sind jz mal alle vorhanden

            RuleFor(type => type.Name).NotNull().WithMessage("Name is required.");
			RuleFor(type => type.DocumentCount).NotNull().WithMessage("Document Count must not be null.");
            RuleFor(type => type.Match).NotNull().WithMessage("Match must not be null.");
            RuleFor(type => type.MatchingAlgorithm).NotNull().WithMessage("Matching Algorithm must not be null.");

        }
    }
}

