using Domain.RepositoryInterfaces;
using Infrastructure.Repositories;
using Infrastructure.Foundation;
using Application.Handlers;
using Application.Commands;
using Application.Services;
using Application.Interfaces.Services;
using Application.Interfaces.Handlers;

namespace KeywordsDashboard
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddDependencies( this IServiceCollection services )
        {
            services.AddScoped<ICommandHandler<AddPageCommand, int?>, AddPageHandler>();
            services.AddScoped<ICommandHandler<DeletePageCommand, int?>, DeletePageHandler>();
            services.AddScoped<ICommandHandler<UpdatePageCommand, int?>, UpdatePageHandler>();
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork<ExtranetPageKeywordsDbContext>>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
