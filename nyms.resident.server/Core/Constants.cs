namespace nyms.resident.server
{
    public static class Constants
    {
        // Config


        // Errors
        public static readonly string TOKEN_INVALID = "INVALID TOKEN: JWT Token Not Valid-1";
        public static readonly string TOKEN_INVALID_IDTY = "INVALID TOKEN: JWT Token Not Valid-2";
        public static readonly string TOKEN_MISSING = "Access denied. Token is Missing";
        public static readonly string TOKEN_ACCESS_DENIED_NO_ROLE_FOR = "Access denied. Role is not granted for ";
        public static readonly string REQUST_ACCESS_FOR = "Requesting Access for ";
        public static readonly string REQUST_ACCESS_GRANTED_FOR = "Access granted for ";

        public static readonly string ENQUIRY_ACTION_STATUS_PENDING = "pending";
        public static readonly string ENQUIRY_ACTION_STATUS_COMPLETED = "completed";
        public static readonly string ENQUIRY_ACTION_STATUS_COMPLETING = "_completing";
    }

    public enum EnquiryStatus
    {
        active,
        closed,
        admit
    }
}