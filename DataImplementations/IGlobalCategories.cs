namespace sti_sys_backend.DataImplementations;

public interface IGlobalCategories
{
    Guid id { get; set; }
    string categoryName { get; set; }
    string categoryDescription { get; set; }
    string categoryPath { get; set; }
    CategoryStatus _categoryStatus { get; set; }
    DateTime created_at { get; set; }
    DateTime updated_at { get; set; }
}

public enum CategoryStatus
{
    ACTIVE,
    INACTIVE
}