using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PrincipalRepro;

var database =  Guid.NewGuid();

var services = new ServiceCollection()
    .AddDbContext<ReproContext>(o => o.UseSqlServer($"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Repro_{database};Integrated Security=True;"))
    .AddScoped<DbContext>(s => s.GetRequiredService<ReproContext>())
    .BuildServiceProvider();

using (var scope = services.CreateScope())
await using (var db = scope.ServiceProvider.GetRequiredService<DbContext>())
{
    await db.Database.EnsureCreatedAsync();

    await db.AddAsync(new EntityB { PKKeyOne = 1, PKKeyTwo = 1, PKKeyThree = 1 }); //Static data created elsewhere
    await db.AddAsync(new EntityA { PKKey = Guid.NewGuid(), FKKeyOne = 1, FKKeyTwo = 1 });

    await db.SaveChangesAsync();
}


using (var scope = services.CreateScope())
await using (var db = scope.ServiceProvider.GetRequiredService<DbContext>())
{
    var existing = await db.Set<EntityA>().Include(e => e.EntityBs)
        .SingleAsync();

    var clone = new EntityA { PKKey = Guid.NewGuid(), FKKeyOne = existing.FKKeyOne, FKKeyTwo = existing.FKKeyTwo};
    clone.Parent = existing;
    await db.AddAsync(clone);


    await db.SaveChangesAsync();
}
