using System.ComponentModel.DataAnnotations;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

/// <summary>
/// Student entity
/// </summary>
public class CatechistEntity
{
    #region Fields

    private readonly List<PhotoReference> _photos = new List<PhotoReference>();

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public CatechistEntity()
    { }

    #endregion

    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Full Name
    /// </summary>
    public FullName FullName { get; set; } = null!;

    /// <summary>
    /// Photo collection
    /// </summary>
    public IReadOnlyCollection<PhotoReference> Photos => _photos;

    #endregion

    #region Methods

    /// <summary>
    /// Add new photo
    /// </summary>
    /// <param name="photo"></param>
    public void AddPhoto(PhotoReference photo)
    {
        _photos.Add(photo);
    }

    /// <summary>
    /// Remove photo
    /// </summary>
    /// <param name="storageKey"></param>
    public void RemovePhoto(string storageKey)
    {
        _photos.RemoveAll(p => p.StorageKey == storageKey);
    }

    #endregion
}
