namespace Aegis.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Error View Model
    /// </summary>
    [DataContract]
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the request identifier.
        /// </summary>
        /// <value>
        /// The request identifier.
        /// </value>
        [DataMember]
        public string? RequestId { get; set; }

        /// <summary>
        /// Gets a value indicating whether [show request identifier].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show request identifier]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}