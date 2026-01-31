using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Domain.Catechism.Members;

public abstract class Member : AggregateRoot<Guid>
{
    #region Fields

    private readonly List<PhotoReference> _photos = new List<PhotoReference>();

    #endregion

    #region Constructors

    /// <summary>
    /// Default
    /// </summary>
    public Member() : base(Guid.Empty)
    {
        Name = new FullName();
    }

    /// <summary>
    /// Constructor from parameters
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    public Member(Guid id, FullName name)
        : base(id)
    {
        Name = name;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Full name
    /// </summary>
    public FullName Name { get; protected set; }

    /// <summary>
    /// Photo list
    /// </summary>
    public IReadOnlyCollection<PhotoReference> Photos => _photos.AsReadOnly();

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

    #endregion
}