using MatrixBugtracker.Abstractions;
using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Entities.Base;
using MatrixBugtracker.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MatrixBugtracker.DAL.Data;

public partial class BugtrackerContext : DbContext
{
    private readonly IConfiguration _config;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserIdProvider _userIdProvider;

    public BugtrackerContext(DbContextOptions<BugtrackerContext> options,
        IConfiguration config,
        IPasswordHasher passwordHasher,
        IUserIdProvider userIdProvider) : base(options)
    {
        _config = config;
        _passwordHasher = passwordHasher;
        _userIdProvider = userIdProvider;
    }

    public virtual DbSet<Comment> Comments { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<CommentAttachment> CommentAttachments { get; set; }
    public virtual DbSet<Confirmation> Confirmations { get; set; }
    public virtual DbSet<UploadedFile> Files { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ProductMember> ProductMembers { get; set; }
    public virtual DbSet<Report> Reports { get; set; }
    public virtual DbSet<ReportAttachment> ReportAttachments { get; set; }
    public virtual DbSet<ReportReproduce> ReportReproduces { get; set; }
    public virtual DbSet<ReportTag> ReportTags { get; set; }
    public virtual DbSet<Tag> Tags { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int userId = _userIdProvider?.UserId ?? 0;

        foreach (var entry in ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Deleted:
                    var deletedEntity = entry.Entity as IDeleteEntity;
                    if (deletedEntity == null) break;
                    deletedEntity.IsDeleted = true;
                    deletedEntity.DeletionTime = DateTime.Now;
                    deletedEntity.DeletedByUserId = userId;
                    entry.State = EntityState.Modified;
                    break;
                case EntityState.Added:
                    var createdEntity = entry.Entity as ICreateEntity;
                    if (createdEntity == null) break;
                    createdEntity.CreationTime = DateTime.Now;
                    createdEntity.CreatorId = userId;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BugtrackerContext).Assembly);

        var firstUser = new User
        {
            Id = 1,
            IsEmailConfirmed = true,
            FirstName = _config["FirstUser:FirstName"],
            LastName = _config["FirstUser:LastName"],
            Email = _config["FirstUser:Email"],
            Password = _passwordHasher.HashPassword(_config["FirstUser:Password"]),
            Role = UserRole.Admin,
            ModeratorName = "Moderator"
        };

        modelBuilder.Entity<User>().HasData(firstUser);
    }
}
