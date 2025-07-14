namespace aspgrupo2.Models
{
    public class UserPrivilege
    {
        public int UserId { get; set; }
        public int PrivilegeId { get; set; }

        public virtual User User { get; set; }
        public virtual Privilege Privilege { get; set; }
    }
}