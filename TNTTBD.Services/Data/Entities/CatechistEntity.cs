using System.ComponentModel.DataAnnotations;

namespace TNTTBD.Services.Data.Entities
{
    /// <summary>
    /// Student entity
    /// </summary>
    public class CatechistEntity : MemberEntity
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CatechistEntity()
        { }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public ICollection<CatechismClassCatechistEntity> ClassAssignments { get; set; } = new List<CatechismClassCatechistEntity>();

        #endregion

    }
}