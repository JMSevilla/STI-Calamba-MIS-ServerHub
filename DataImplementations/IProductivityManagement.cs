namespace sti_sys_backend.DataImplementations;

public interface IProductivityManagement
{
    Guid id { get; set; }
    int accountId { get; set; }
    ProductivityStatus _productivityStatus { get; set; }
    TimeSpan TimeIn { get; set; }
    TimeSpan TimeOut { get; set; }
    Status _status { get; set; }
    DateTime Date { get; set; }
}

public enum ProductivityStatus
{
    PENDING,
    LATE,
    ABSENT,
    ONTIME
}

public enum Status
{
    TIME_IN,
    TIME_OUT
}