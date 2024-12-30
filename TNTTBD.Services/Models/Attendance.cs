using TNTTBD.Services.Models.Enums;

namespace TNTTBD.Services.Models 
{
    /// <summary>
    /// Attendance entity
    /// </summary>
    public class Attendance 
    {
        #region Constructors
        #endregion

        #region Properties

        /// <summary>
        /// Attendance Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Member
        /// </summary>
        public Member Member { get; set; }

        /// <summary>
        /// Session
        /// </summary>
        public CatechismSession CatechismSession { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public AttendanceStatus AttendanceStatus{ get; set; }

        #endregion
    }
}