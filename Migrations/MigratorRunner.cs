using FluentMigrator.Runner;
using FluentMigrator.Runner.VersionTableInfo;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Migrations;

public class MigratorRunner(string connectionString)
{
    public void Migrate()
    {
        var serviceProvider = CreateServices(); //собираем DI-контейнер

        using var scope = serviceProvider.CreateScope(); //область жизни зависимостей
        UpdateDatabase(serviceProvider.GetRequiredService<IMigrationRunner>()); //manual resolving зависимости, не автоматический
    }

    private IServiceProvider CreateServices()
    {
        Console.WriteLine(typeof(MigratorRunner).Assembly.FullName);
         
        return new ServiceCollection() // создаём новый контейнер зависимостей
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(MigratorRunner).Assembly).For.Migrations()) //классы миграций в текущей сборке
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .AddScoped<IVersionTableMetaData, VersionTable>()
            .BuildServiceProvider(false);
    }

    private void UpdateDatabase(IMigrationRunner runner)
    {
        // Мигрируем базу
        runner.MigrateUp();
        // создаем и открываем коннект к бд
        using var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        // перегружаем композитные типы
        connection.ReloadTypes();
    }
}