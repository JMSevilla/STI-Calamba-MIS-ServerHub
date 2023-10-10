namespace sti_sys_backend.DataImplementations
{
    public interface ITicketIssues
    {
        public Guid Id { get; set; }
        public string issue { get; set; }
        public string issueKey { get; set; }
        public int status { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
