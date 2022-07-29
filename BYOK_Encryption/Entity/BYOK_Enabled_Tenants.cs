namespace BYOK_Encryption.Entity
{
    public class BYOK_Enabled_Tenants
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string Table_name { get; set; }
        public string Column_name { get; set; }
        public bool IsEntireDbEncrypted { get; set; }

    }
}
