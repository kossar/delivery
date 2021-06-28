using System;

namespace WebApp.Models
{
    /// <summary>
    /// Error View Model
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Request Id
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Show requestId if RequestId has value and is not empty
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}