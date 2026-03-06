using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Domain.Catechism.Members;

public abstract class Member : AggregateRoot<Guid>
{
    #region Fields

    private readonly List<PhotoReference> _photos = new List<PhotoReference>();
    private readonly List<PatronSaint> _patronSaints = new List<PatronSaint>();

    #endregion

    #region Constructors

    /// <summary>
    /// Default
    /// </summary>
    public Member() : base(Guid.Empty)
    {
        FullName = new FullName();
        Address = string.Empty;
    }

    /// <summary>
    /// Constructor from parameters
    /// </summary>
    public Member(Guid id, FullName name, DateOnly dateOfBirth, string? address, bool isMale) : base(id)
    {
        FullName = name;
        DateOfBirth = dateOfBirth;
        IsMale = isMale;
        Address = address ?? string.Empty;
    }

    /// <summary>
    /// Constructor from parameters
    /// </summary>
    public Member(Guid id, FullName name, DateOnly dateOfBirth, string? address, bool isMale, IEnumerable<PhotoReference> photos, IEnumerable<PatronSaint> patronSaints) : base(id)
    {
        FullName = name;
        _photos.AddRange(photos);
        DateOfBirth = dateOfBirth;
        
        _patronSaints.AddRange(patronSaints);
        IsMale = isMale;
        Address = address ?? string.Empty;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Full name
    /// </summary>
    public FullName FullName { get; protected set; }

    /// <summary>
    /// Date Of Birth
    /// </summary>
    public DateOnly DateOfBirth { get; protected set; }

    /// <summary>
    /// Address
    /// </summary>
    public string Address { get; private set; }

    /// <summary>
    /// Is male
    /// </summary>
    public bool IsMale { get; protected set; }

    /// <summary>
    /// Photo list
    /// </summary>
    public IReadOnlyCollection<PhotoReference> Photos => _photos.AsReadOnly();

    /// <summary>
    /// Patron Saints
    /// </summary>
    public IReadOnlyCollection<PatronSaint> PatronSaints => _patronSaints.AsReadOnly();

    #endregion

    #region Methods

    /// <summary>
    /// Add photo
    /// </summary>
    /// <param name="photo"></param>
    public void AddPhoto(PhotoReference photo)
    {
        if (photo.Purpose == PhotoPurpose.Profile)
        {
            _photos.RemoveAll(p => p.Purpose == PhotoPurpose.Profile);
        }

        _photos.Add(photo);
    }

    /// <summary>
    /// Assign patron saint
    /// </summary>
    public void AddPatronSaint(PatronSaint patronSaint)
    {
        if (_patronSaints.Any(s => s.SaintName == patronSaint.SaintName && s.Day == patronSaint.Day && s.Month == patronSaint.Month))
        {
            throw new DomainException(DomainErrors.PatronSaintAlreadyAssigned);
        }

        _patronSaints.Add(patronSaint);
    }

    /// <summary>
    /// Change name
    /// </summary>
    public void ChangeName(FullName name)
    {
        if (name == null)
        {
            throw new DomainException(DomainErrors.ValidationRequired);
        }

        FullName = name;
    }

    /// <summary>
    /// Update basic info
    /// </summary>
    public void UpdateInfo(FullName fullName, DateOnly dateOfBirth, string address, bool isMale)
    {
        FullName = fullName;
        DateOfBirth = dateOfBirth;
        Address = address;
        IsMale = isMale;
    }

    #endregion
}