using System;
using Elsa.Webhooks.Persistence.EntityFramework.Core.Services;
using Elsa.Webhooks.Persistence.EntityFramework.Core.StartupTasks;
using Elsa.Webhooks.Persistence.EntityFramework.Core.Stores;
using Elsa.Activities.Webhooks;
using Elsa.Runtime;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Webhooks.Persistence.EntityFramework.Core.Extensions
{
    public static class WebhookServiceCollectionExtensions
    {
        /// <summary>
        /// Configures Elsa to use Entity Framework Core for persistence, using pooled DB Context instances.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Pooled DB Context instances is a performance optimisation which is documented in more detail at
        /// https://docs.microsoft.com/en-us/ef/core/performance/advanced-performance-topics?tabs=with-constant#dbcontext-pooling.
        /// </para>
        /// </remarks>
        /// <param name="webhookOptions">An Elsa options builder</param>
        /// <param name="configure">A configuration builder callback</param>
        /// <param name="autoRunMigrations">If <c>true</c> then database migrations will be auto-executed on startup</param>
        /// <returns>The Elsa options builder, so calls may be chained</returns>
        public static WebhookOptionsBuilder UseEntityFrameworkPersistence(this WebhookOptionsBuilder webhookOptions,
            Action<DbContextOptionsBuilder> configure,
            bool autoRunMigrations = true) =>
            webhookOptions.UseEntityFrameworkPersistence<WebhookContext>(configure, autoRunMigrations);

        /// <summary>
        /// Configures Elsa to use Entity Framework Core for persistence, using pooled DB Context instances.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Pooled DB Context instances is a performance optimisation which is documented in more detail at
        /// https://docs.microsoft.com/en-us/ef/core/performance/advanced-performance-topics?tabs=with-constant#dbcontext-pooling.
        /// </para>
        /// </remarks>
        /// <param name="webhookOptions">An Elsa options builder</param>
        /// <param name="configure">A configuration builder callback</param>
        /// <param name="autoRunMigrations">If <c>true</c> then database migrations will be auto-executed on startup</param>
        /// <typeparam name="TElsaContext">The concrete type of <see cref="ElsaContext"/> to use.</typeparam>
        /// <returns>The Elsa options builder, so calls may be chained</returns>
        public static WebhookOptionsBuilder UseEntityFrameworkPersistence<TElsaContext>(this WebhookOptionsBuilder webhookOptions,
            Action<DbContextOptionsBuilder> configure,
            bool autoRunMigrations = true) where TElsaContext : WebhookContext =>
            webhookOptions.UseEntityFrameworkPersistence<TElsaContext>((_, builder) => configure(builder), autoRunMigrations);

        /// <summary>
        /// Configures Elsa to use Entity Framework Core for persistence, using pooled DB Context instances.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Pooled DB Context instances is a performance optimisation which is documented in more detail at
        /// https://docs.microsoft.com/en-us/ef/core/performance/advanced-performance-topics?tabs=with-constant#dbcontext-pooling.
        /// </para>
        /// </remarks>
        /// <param name="webhookOptions">An Elsa options builder</param>
        /// <param name="configure">A configuration builder callback, which also provides access to a service provider</param>
        /// <param name="autoRunMigrations">If <c>true</c> then database migrations will be auto-executed on startup</param>
        /// <returns>The Elsa options builder, so calls may be chained</returns>
        public static WebhookOptionsBuilder UseEntityFrameworkPersistence(this WebhookOptionsBuilder webhookOptions,
            Action<IServiceProvider, DbContextOptionsBuilder> configure,
            bool autoRunMigrations = true) =>
            webhookOptions.UseEntityFrameworkPersistence<WebhookContext>(configure, autoRunMigrations);

        /// <summary>
        /// Configures Elsa to use Entity Framework Core for persistence, using pooled DB Context instances.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Pooled DB Context instances is a performance optimisation which is documented in more detail at
        /// https://docs.microsoft.com/en-us/ef/core/performance/advanced-performance-topics?tabs=with-constant#dbcontext-pooling.
        /// </para>
        /// </remarks>
        /// <param name="webhookOptions">An Elsa options builder</param>
        /// <param name="configure">A configuration builder callback, which also provides access to a service provider</param>
        /// <param name="autoRunMigrations">If <c>true</c> then database migrations will be auto-executed on startup</param>
        /// <typeparam name="TElsaContext">The concrete type of <see cref="ElsaContext"/> to use.</typeparam>
        /// <returns>The Elsa options builder, so calls may be chained</returns>
        public static WebhookOptionsBuilder UseEntityFrameworkPersistence<TElsaContext>(this WebhookOptionsBuilder webhookOptions,
            Action<IServiceProvider, DbContextOptionsBuilder> configure,
            bool autoRunMigrations = true) where TElsaContext : WebhookContext =>
            UseEntityFrameworkPersistence<TElsaContext>(webhookOptions, configure, autoRunMigrations, true, ServiceLifetime.Singleton);

        /// <summary>
        /// Configures Elsa to use Entity Framework Core for persistence, without using pooled DB Context instances.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method when you do not wish to use DB connection pooling, such as when integrating with a multi-tenant
        /// application, where re-use of DB Context objects is impractical.
        /// </para>
        /// <para>
        /// Although auto-running of migrations is supported in this scenario, use this with caution. When pooling is not in use and each instance of
        /// the DB Context may differ, it is not feasible to try to automatically migrate them.
        /// Your application is ultimately responsible for executing the contents of the <see cref="RunMigrations"/> class in a manner
        /// which is suitable for your use-case.
        /// </para>
        /// </remarks>
        /// <param name="webhookOptions">An Elsa options builder</param>
        /// <param name="configure">A configuration builder callback</param>
        /// <param name="serviceLifetime">The service lifetime which will be used for each DB Context instance</param>
        /// <param name="autoRunMigrations">If <c>true</c> then database migrations will be auto-executed on startup</param>
        /// <returns>The Elsa options builder, so calls may be chained</returns>
        public static WebhookOptionsBuilder UseNonPooledEntityFrameworkPersistence(this WebhookOptionsBuilder webhookOptions,
            Action<DbContextOptionsBuilder> configure,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton,
            bool autoRunMigrations = false) =>
            webhookOptions.UseNonPooledEntityFrameworkPersistence<WebhookContext>(configure, serviceLifetime, autoRunMigrations);

        /// <summary>
        /// Configures Elsa to use Entity Framework Core for persistence, without using pooled DB Context instances.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method when you do not wish to use DB connection pooling, such as when integrating with a multi-tenant
        /// application, where re-use of DB Context objects is impractical.
        /// </para>
        /// <para>
        /// Although auto-running of migrations is supported in this scenario, use this with caution. When pooling is not in use and each instance of
        /// the DB Context may differ, it is not feasible to try to automatically migrate them.
        /// Your application is ultimately responsible for executing the contents of the <see cref="RunMigrations"/> class in a manner
        /// which is suitable for your use-case.
        /// </para>
        /// </remarks>
        /// <param name="webhookOptions">An Elsa options builder</param>
        /// <param name="configure">A configuration builder callback</param>
        /// <param name="serviceLifetime">The service lifetime which will be used for each DB Context instance</param>
        /// <param name="autoRunMigrations">If <c>true</c> then database migrations will be auto-executed on startup</param>
        /// <typeparam name="TElsaContext">The concrete type of <see cref="ElsaContext"/> to use.</typeparam>
        /// <returns>The Elsa options builder, so calls may be chained</returns>
        public static WebhookOptionsBuilder UseNonPooledEntityFrameworkPersistence<TElsaContext>(this WebhookOptionsBuilder webhookOptions,
            Action<DbContextOptionsBuilder> configure,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton,
            bool autoRunMigrations = false) where TElsaContext : WebhookContext =>
            webhookOptions.UseNonPooledEntityFrameworkPersistence<TElsaContext>((_, builder) => configure(builder), serviceLifetime, autoRunMigrations);

        /// <summary>
        /// Configures Elsa to use Entity Framework Core for persistence, without using pooled DB Context instances.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method when you do not wish to use DB connection pooling, such as when integrating with a multi-tenant
        /// application, where re-use of DB Context objects is impractical.
        /// </para>
        /// <para>
        /// Although auto-running of migrations is supported in this scenario, use this with caution. When pooling is not in use and each instance of
        /// the DB Context may differ, it is not feasible to try to automatically migrate them.
        /// Your application is ultimately responsible for executing the contents of the <see cref="RunMigrations"/> class in a manner
        /// which is suitable for your use-case.
        /// </para>
        /// </remarks>
        /// <param name="webhookOptions">An Elsa options builder</param>
        /// <param name="configure">A configuration builder callback, which also provides access to a service provider</param>
        /// <param name="serviceLifetime">The service lifetime which will be used for each DB Context instance</param>
        /// <param name="autoRunMigrations">If <c>true</c> then database migrations will be auto-executed on startup</param>
        /// <returns>The Elsa options builder, so calls may be chained</returns>
        public static WebhookOptionsBuilder UseNonPooledEntityFrameworkPersistence(this WebhookOptionsBuilder webhookOptions,
            Action<IServiceProvider, DbContextOptionsBuilder> configure,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton,
            bool autoRunMigrations = false) =>
            webhookOptions.UseNonPooledEntityFrameworkPersistence<WebhookContext>(configure, serviceLifetime, autoRunMigrations);

        /// <summary>
        /// Configures Elsa to use Entity Framework Core for persistence, without using pooled DB Context instances.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method when you do not wish to use DB connection pooling, such as when integrating with a multi-tenant
        /// application, where re-use of DB Context objects is impractical.
        /// </para>
        /// <para>
        /// Although auto-running of migrations is supported in this scenario, use this with caution. When pooling is not in use and each instance of
        /// the DB Context may differ, it is not feasible to try to automatically migrate them.
        /// Your application is ultimately responsible for executing the contents of the <see cref="RunMigrations"/> class in a manner
        /// which is suitable for your use-case.
        /// </para>
        /// </remarks>
        /// <param name="webhookOptions">An Elsa options builder</param>
        /// <param name="configure">A configuration builder callback, which also provides access to a service provider</param>
        /// <param name="serviceLifetime">The service lifetime which will be used for each DB Context instance</param>
        /// <param name="autoRunMigrations">If <c>true</c> then database migrations will be auto-executed on startup</param>
        /// <typeparam name="TElsaContext">The concrete type of <see cref="ElsaContext"/> to use.</typeparam>
        /// <returns>The Elsa options builder, so calls may be chained</returns>
        public static WebhookOptionsBuilder UseNonPooledEntityFrameworkPersistence<TElsaContext>(this WebhookOptionsBuilder webhookOptions,
            Action<IServiceProvider, DbContextOptionsBuilder> configure,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton,
            bool autoRunMigrations = false) where TElsaContext : WebhookContext =>
            UseEntityFrameworkPersistence<TElsaContext>(webhookOptions, configure, autoRunMigrations, false, serviceLifetime);

        static WebhookOptionsBuilder UseEntityFrameworkPersistence<TElsaContext>(WebhookOptionsBuilder webhookOptions,
            Action<IServiceProvider, DbContextOptionsBuilder> configure,
            bool autoRunMigrations,
            bool useContextPooling,
            ServiceLifetime serviceLifetime) where TElsaContext : WebhookContext
        {
            /* Auto-running migrations is intentionally unavailable when not using context pooling.
             * When we aren't using pooling then it probably means that each DB Context is different
             * in some manner.  That could easily mean the connection strings (IE: Contexts might not
             * all connect to the same DB).  In that case, without further logic (which can't be
             * pre-empted by Elsa), we can't be sure we're connecting to the right DBs when running
             * migrations.
             *
             * It's much more sane just to explicitly not-support it and leave it to the app developer.
             * They can run their own migrations in line with their own logic.
             */

            if (useContextPooling)
                webhookOptions.Services.AddPooledDbContextFactory<TElsaContext>(configure);
            else
                webhookOptions.Services.AddDbContextFactory<TElsaContext>(configure, serviceLifetime);

            webhookOptions.Services
                .AddSingleton<IWebhookContextFactory, WebhookContextFactory<TElsaContext>>()
                .AddScoped<EntityFrameworkWebhookDefinitionStore>();

            if (autoRunMigrations)
                webhookOptions.Services.AddStartupTask<RunMigrations>();

            webhookOptions.UseWebhookDefinitionStore(sp =>
            {
                var x = sp.GetRequiredService<EntityFrameworkWebhookDefinitionStore>();
                return x;
            });

            return webhookOptions;
        }
    }
}
