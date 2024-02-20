using Microsoft.AspNetCore.Identity;

namespace Dstl.Afst.Core.Data.Identity
{
    /// <summary>
    /// Holds informaion about custom Identity Errors
    /// </summary>
    public static class CustomIdentityError
    {
        /// <summary>
        /// To be used when a user with matching details already exists in the database.
        /// </summary>
        public static IdentityError USER_ALREADY_EXISTS => new()
        {
            Code = "UserAlreadyExists",
            Description = "User already exists."
        };

        /// <summary>
        /// To be used when the supplied name is too short.
        /// </summary>
        /// <param name="minimum">The minimum length the name should be.</param>
        public static IdentityError NAME_TOO_SHORT(int minimum) => new()
        {
            Code = "NameTooShort",
            Description = $"Supplied name must be at least {minimum} characters."
        };

        /// <summary>
        /// To be used when the supplied name contains invalid characters.
        /// </summary>
        public static IdentityError NAME_INVALID => new()
        {
            Code = "NameTooShort",
            Description = $"Supplied name contains illegal characters."
        };

        /// <summary>
        /// To be used when the supplied email is invalid.
        /// </summary>
        public static IdentityError EMAIL_INVALID => new()
        {
            Code = "EmailInvalid",
            Description = "Email is not a valid format."
        };

        /// <summary>
        /// To be used when the supplied password is too short.
        /// </summary>
        /// <param name="minimum">The minimum length the password should be.</param>
        public static IdentityError PASSWORD_TOO_SHORT(int length) => new()
        {
            Code = "PasswordTooShort",
            Description = $"Password must be at least {length} characters."
        };

        /// <summary>
        /// To be used when the supplied password is missing numeric characters.
        /// </summary>
        /// <param name="minimum">The minimum number of numerical characters that should be present.</param>
        public static IdentityError PASSWORD_MISSING_NUMERIC(int minimum) => new()
        {
            Code = "PasswordMissingNumeric",
            Description = $"Password must contain at least {minimum} numeric values."
        };

        /// <summary>
        /// To be used when the supplied password is missing symbols.
        /// </summary>
        /// <param name="minimum">The minimum number of symbols that should be present.</param>
        public static IdentityError PASSWORD_MISSING_SYMBOL(int minimum) => new()
        {
            Code = "PasswordMissingSymbol",
            Description = $"Password must contain at least {minimum} symbols."
        };

        /// <summary>
        /// To be used when the supplied password is missing uppercase characters.
        /// </summary>
        /// <param name="minimum">The minimum number of uppercase characters that should be present.</param>
        public static IdentityError PASSWORD_MISSING_UPPER(int minimum) => new()
        {
            Code = "PasswordMissingSymbol",
            Description = $"Password must contain at least {minimum} uppercase characters."
        };
    }
}
