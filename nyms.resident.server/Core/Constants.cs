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

        // spends
        public static readonly string BUDGET_APPROVED = "Y";
        // budget status
        public static readonly string BUDGET_STATUS_CREATED = "Created";
        public static readonly string BUDGET_STATUS_OPEN = "Open";
        public static readonly string BUDGET_STATUS_COMPLETED = "Completed";
        public static readonly string BUDGET_STATUS_CANCELLED = "Cancelled";

        public static readonly string SPEND_TRAN_TYPE_DEBIT = "Debit";
        public static readonly string SPEND_TRAN_TYPE_CREDIT = "Credit";

        // Meetings
        public static readonly string MEETING_ACTION_DELETED = "Deleted";
    }

    public enum ENQUIRY_STATUS
    {
        active,
        closed,
        admit
    }
    public enum REF_TYPE
    {
        resident,
        nok
    }
    public enum CONTACT_TYPE
    {
        email,
        phone
    }
    public enum ADDRESS_TYPE
    {
       home
    }

    public enum USER_ROLE
    {
        SuperAdmin = 1,
        Admin = 2,
        Manger = 3,
        FinanceAdmin = 4
    }

    
}