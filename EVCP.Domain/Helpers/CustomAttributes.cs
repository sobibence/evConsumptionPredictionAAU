namespace EVCP.Domain.Helpers;

/// <summary>
/// Custom attribute in order to be able to ignore certain properties in sql insert queries.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class OnInsertIgnore : Attribute
{
}

/// <summary>
/// Mark string property as enum to recognize it in sql insert queries.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class EnumType : Attribute
{
}

/// <summary>
/// Table name of database entity
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class TableName : Attribute
{
    public string Name { get; }

    public TableName(string name)
    {
        Name = name;
    }
}

/// <summary>
/// Column name in database table
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ColumnName : Attribute
{
    public string Name { get; }

    public ColumnName(string name)
    {
        Name = name;
    }
}
