using System;

namespace UrlShortener.Services
{
    /// <summary>
    /// Represents errors that occur during URL shortening operations.
    /// </summary>
    public class UrlShorteningException : Exception
    {
        /// <summary>
        /// Gets the invalid URL that caused the exception.
        /// </summary>
        public string InvalidUrl { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlShorteningException"/> class with a specified error message and the invalid URL.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="invalidUrl">The invalid URL that caused the exception.</param>
        public UrlShorteningException(string message, string invalidUrl)
            : base(message)
        {
            InvalidUrl = invalidUrl;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlShorteningException"/> class with a specified error message, the invalid URL, and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="invalidUrl">The invalid URL that caused the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public UrlShorteningException(string message, string invalidUrl, Exception inner)
            : base(message, inner)
        {
            InvalidUrl = invalidUrl;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlShorteningException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public UrlShorteningException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Returns a string representation of the current exception.
        /// </summary>
        /// <returns>A string that represents the current exception.</returns>
        public override string ToString()
        {
            return InvalidUrl == null
                ? base.ToString()
                : $"{base.ToString()}, Invalid URL: {InvalidUrl}";
        }
    }
}