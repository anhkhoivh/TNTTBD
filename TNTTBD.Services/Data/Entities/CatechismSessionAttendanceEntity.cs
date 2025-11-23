using System.ComponentModel.DataAnnotations;
using TNTTBD.Services.Models.Enums;

namespace TNTTBD.Services.Data.Entities
{
    /// <summary>
    /// Attendance entity
    /// </summary>
    public class CatechismSessionAttendanceEntity 
    {
        #region Constructors
        #endregion

        #region Properties

        /// <summary>
        /// Attendance Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Id of member
        /// </summary>
        public Guid MemberId { get; set; }

        /// <summary>
        /// Id of session
        /// </summary>
        public Guid CatechismSessionId { get; set; }

        /// <summary>
        /// Session
        /// </summary>
        public CatechismSessionEntity CatechismSession { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public AttendanceStatus AttendanceStatus{ get; set; }

        #endregion
    }
}