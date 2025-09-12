namespace MeskChatApplication.Application.Notifications.Email;

public static class ForgotPasswordTemplate
{
    public const string Value = """
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Password Reset</title>
        </head>
        <body style=""font-family: Arial, sans-serif; background-color: #f9f9f9; padding: 20px;"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; overflow: hidden;"">
                <tr>
                    <td style=""background-color: #4CAF50; color: #ffffff; padding: 20px; text-align: center; font-size: 24px;"">
                        Password Reset Request
                    </td>
                </tr>
                <tr>
                    <td style=""padding: 30px; color: #333333;"">
                        <p>Hello,</p>
                        <p>We received a request to reset your password. Click the button below to set a new password:</p>

                        <p style=""text-align: center; margin: 30px 0;"">
                            <a href=""{{resetLink}}"" 
                               style=""background-color: #4CAF50; color: #ffffff; text-decoration: none; 
                                      padding: 12px 24px; border-radius: 5px; font-weight: bold;"">
                                Reset Password
                            </a>
                        </p>

                        <p>If the button above does not work, copy and paste the following link into your browser:</p>
                        <p style=""word-break: break-all;"">
                            <a href=""{{resetLink}}"">{{resetLink}}</a>
                        </p>

                        <p>If you did not request a password reset, you can safely ignore this email.</p>

                        <p style=""margin-top: 40px;"">Thanks,<br/>The Support Team</p>
                    </td>
                </tr>
                <tr>
                    <td style=""background-color: #f1f1f1; color: #777777; font-size: 12px; text-align: center; padding: 15px;"">
                        © 2025 Your Company. All rights reserved.
                    </td>
                </tr>
            </table>
        </body>
        </html>
    """;
}
