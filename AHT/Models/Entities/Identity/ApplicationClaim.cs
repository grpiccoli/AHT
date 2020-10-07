using System.Collections.Generic;

namespace AHT.Models.Entities.Identity
{
    public class ApplicationClaim
    {
        public static List<string> UserClaims { get; } = new List<string>
        {
            "ServiceRequest",
            "ServicePayment",
            "ReportDownload",
            "ReportHistory"
        };
    }
}
