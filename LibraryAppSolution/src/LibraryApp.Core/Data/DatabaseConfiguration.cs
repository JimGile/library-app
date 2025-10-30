namespace LibraryApp.Core.Data;

/// <summary>
/// Configuration class for database connection strings.
/// </summary>
public static class DatabaseConfiguration
{
    /// <summary>
    /// Gets the SQLite connection string for development.
    /// </summary>
    public static string DevelopmentConnectionString => "Data Source=libraryapp.db";

    /// <summary>
    /// Gets the SQL Server connection string for production.
    /// Note: This should be configured via environment variables or configuration files in production.
    /// </summary>
    public static string ProductionConnectionString =>
        Environment.GetEnvironmentVariable("LIBRARY_DB_CONNECTION") ??
        "Server=(localdb)\\mssqllocaldb;Database=LibraryApp;Trusted_Connection=True;MultipleActiveResultSets=true";

    /// <summary>
    /// Gets the appropriate connection string based on environment.
    /// </summary>
    /// <param name="isDevelopment">Whether the application is running in development mode.</param>
    /// <returns>The connection string.</returns>
    public static string GetConnectionString(bool isDevelopment = true)
    {
        return isDevelopment ? DevelopmentConnectionString : ProductionConnectionString;
    }
}