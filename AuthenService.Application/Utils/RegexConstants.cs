using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenService.Application.Utils
{
    public static class RegexConstants
    {
        /// <summary>
        /// Regex for Validating International Format Phone Numbers
        /// </summary>
        public const string PhoneNumberRegex = @"^\+[0-9]?()[0-9](\S)(\d[0-9]{8,14})$";

        /// <summary>
        /// Regex for Identifying a Valid Email Address
        /// </summary>
        public const string EmailRegex = @"\A(?:[a-zA-Z0-9!#$%&'*+\/=?^_{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+\/=?^_{|}~-]+)@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-][a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)\Z";

        /// <summary>
        /// Regex For Password to contain at least one lowercase alphabet, one uppercase alphabet, one special character, one number digit and must be at least 8 characters long
        /// </summary>
        public const string PasswordRegex = @"^(?=.[a-z])(?=.[A-Z])(?=.\d)(?=.[^a-zA-Z\d\s:]).{8,}$";

        /// <summary>
        /// Regex for Validation 2FA Token to be 6 digits long
        /// </summary>
        public const string TokenRegex = @"^[0-9]{6}$";

        /// <summary>
        /// Regex For Validating Full Name
        /// </summary>
        public const string FullNameRegex = @"^(?![\s.]+$)[a-zA-Z][a-zA-Z\s.-]*[a-zA-Z]$";

        /// <summary>
        /// Regex for validating text fields
        /// </summary>
        public const string TextRegex = @"^[\w\.,:\?&=/\-\+_)( ]*$";
    }
}
