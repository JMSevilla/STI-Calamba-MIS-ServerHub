namespace sti_sys_backend.DataImplementations;

public interface JitsiPrivateKeyStorage
{
    Guid id { get; set; }
    byte[] PrivateKey { get; set; }
    int active { get; set; }
}