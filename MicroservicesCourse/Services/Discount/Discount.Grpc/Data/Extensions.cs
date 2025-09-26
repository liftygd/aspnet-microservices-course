using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public static class Extensions
    {
        public static void EnsureMigrationOf<T>(this IApplicationBuilder app) where T : DbContext
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<T>();

            if (context == null)
                throw new NotImplementedException($"Context of type '{typeof(T).Name}' was not added to application.");

            context.Database.MigrateAsync();
        }
    }
}
