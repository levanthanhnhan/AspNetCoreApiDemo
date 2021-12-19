using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Common
{
    public class EmailTemplate
    {
        public const string RESET_PASSWORD = @"<h1> Reset password </h1>" +
                                              "<p>Hi {0}, <p>" +
                                              "<p>Request has been received to change the password for your account.</p>" +
                                              "<p>New password: <span style='font-weight: bold'>{1}</span><br /></p>" +
                                              "<p>Thanks, </p>";

        public const string ACTIVATION = @"<h1> MAIL ACTIVATION </h1>" +
                                              "<p>Hi {0}, <p>" +
                                              "<p>Your account has been registered. Please click link below to complete</p>" +
                                              "<p>Login User: {1}" +
                                              "<p>Login Password: {2}" +
                                              "<p>Link: <a href = '{3}'>ACTIVE</a>" +
                                              "<p>Thanks, </p>";
    }
}
