namespace sti_sys_backend.DataImplementations;

public interface IActionsLogger
{
    Guid id { get; set; }
    int accountId { get; set; }
    string actionsMessage { get; set; }
    DateTime created_at { get; set; }
    DateTime updated_at { get; set; }
}