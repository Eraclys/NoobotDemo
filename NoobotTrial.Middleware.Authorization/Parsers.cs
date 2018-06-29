using Sprache;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NoobotTrial.Middleware.Authorization
{
    public static class PermissionCommandParser
    {
        private static Parser<IEnumerable<char>> AuthKeyWord =>
            Parse.String("auth");
        
        private static Parser<IEnumerable<char>> ListingKeyword =>
            Parse.String("ls");

        private static Parser<string> EmailAddress => 
            Parse.Regex(new Regex("[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,}", RegexOptions.Compiled | RegexOptions.IgnoreCase));

        private static Parser<string> FormattedEmailAddress =>
            from _ in Parse.String("<mailto:")
            from email in EmailAddress
            from __ in Parse.String("|")
            from email2 in EmailAddress
            from ___ in Parse.String(">")
            select email;

        private static Parser<IEnumerable<string>> CommadDelimitedEmailAddressList =>
            FormattedEmailAddress.Or(EmailAddress).Token().DelimitedBy(Parse.Chars(","));

        private static Parser<string> PermissionName =>
            Parse.Regex(new Regex("[a-z0-9-]+", RegexOptions.Compiled | RegexOptions.IgnoreCase))
                .Named("PermissionName");

        private static Parser<OperationType> Operator(string op, OperationType opType) =>
            Parse.String(op).Return(opType);

        private static Parser<OperationType> Grant =>
            Operator("grant", OperationType.Grant);

        private static Parser<OperationType> Deny =>
            Operator("deny", OperationType.Deny);

        private static Parser<OperationType> Operation =>
            Grant.Or(Deny).Named("Operation");
        
        private static Parser<PermissionCommand> PermissionCommandExpression =>
            from _ in AuthKeyWord
            from __ in Parse.WhiteSpace.AtLeastOnce()
            from op in Operation
            from ___ in Parse.WhiteSpace.AtLeastOnce()
            from perm in PermissionName
            from ____ in Parse.WhiteSpace.AtLeastOnce()
            from emails in CommadDelimitedEmailAddressList
            select new PermissionCommand(op, perm.ToLower(), emails.Select(e => e.ToLower()));
        
        private static Parser<string> PermissionsByUserExpression =>
            from _ in AuthKeyWord
            from __ in Parse.WhiteSpace.AtLeastOnce()
            from ___ in ListingKeyword
            from ____ in Parse.WhiteSpace.AtLeastOnce()
            from _____ in Parse.String("-u")
            from ______ in Parse.WhiteSpace.AtLeastOnce()
            from email in FormattedEmailAddress.Or(EmailAddress)
            select email;

        private static Parser<string> UsersByPermissionExpression =>
            from _ in AuthKeyWord
            from __ in Parse.WhiteSpace.AtLeastOnce()
            from ___ in ListingKeyword
            from ____ in Parse.WhiteSpace.AtLeastOnce()
            from _____ in Parse.String("-p")
            from ______ in Parse.WhiteSpace.AtLeastOnce()
            from permissionName in PermissionName
            select permissionName;

        public static IResult<string> TryParsePermissionsByUserExpression(string text) =>
            PermissionsByUserExpression.End().TryParse(text);
        
        public static string ParsePermissionsByUserExpression(string text) =>
            PermissionsByUserExpression.End().Parse(text);

        public static IResult<string> TryParseUsersByPermissionExpression(string text) =>
            UsersByPermissionExpression.End().TryParse(text);

        public static string ParseUsersByPermissionExpression(string text) =>
            UsersByPermissionExpression.End().Parse(text);

        public static IResult<PermissionCommand> TryParsePermissionCommandExpression(string text) =>
            PermissionCommandExpression.End().TryParse(text);

        public static PermissionCommand ParsePermissionCommandExpression(string text) =>
            PermissionCommandExpression.End().Parse(text);
    }
}
