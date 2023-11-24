using System;
using FluentValidation;
using Paperless.BusinessLogic.Entities;

namespace Paperless.BusinessLogic
{
	public class DocumentValidator : AbstractValidator<Document>
	{
		public DocumentValidator()
		{
            // nochmal checken welche properties alle required sind..., hier sind jz mal alle vorhanden

            RuleFor(document => document.Id).NotNull().WithMessage("Id is required.");
            RuleFor(document => document.Correspondent).NotNull().WithMessage("Correspondent is required.");
			RuleFor(document => document.DocumentType).NotNull().WithMessage("DocumentType must not be null.");
            RuleFor(document => document.StoragePath).NotNull().WithMessage("StoragePath must not be null.");
            RuleFor(document => document.Title).NotNull().WithMessage("Title must not be null.");
            RuleFor(document => document.Content).NotNull().WithMessage("Content must not be null.");
            RuleFor(document => document.Tags).NotNull().WithMessage("Tags must not be null.");
            RuleFor(document => document.Created).NotNull().WithMessage("Created must not be null.");
            RuleFor(document => document.Modified).NotNull().WithMessage("Modified must not be null.");
            RuleFor(document => document.Added).NotNull().WithMessage("Added must not be null.");
            RuleFor(document => document.ArchiveSerialNumber).NotNull().WithMessage("ArchivedFileName must not be null.");
            RuleFor(document => document.ArchivedFileName).NotNull().WithMessage("ArchivedFileName must not be null.");
            RuleFor(document => document.OriginalFileName).NotNull().WithMessage("OriginalFileName must not be null.");

        }
    }
}

