using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.EucharisticYouths.Commands;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

/// <summary>
/// Add milestone handler
/// </summary>
public class AddPatronSaintHandler : IRequestHandler<AddPatronSaintCommand>
{
    #region Fields

    private readonly IEucharisticYouthRepository _repository;
    private readonly IUnitOfWork  _unitOfWork;

    #endregion

    #region Constructors

    public AddPatronSaintHandler(IEucharisticYouthRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Methods

    public async Task Handle(AddPatronSaintCommand command, CancellationToken cancellationToken)
    {
        PatronSaint patronSaint = new(command.SaintName, command.Day, command.Month);
        await _repository.AddPatronSaint(command.EucharisticYouthId, patronSaint, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    #endregion
}