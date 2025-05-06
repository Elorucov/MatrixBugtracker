using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Entities.Base;
using MatrixBugtracker.DAL.ProviderInterfaces;
using Microsoft.EntityFrameworkCore;

namespace MatrixBugtracker.DAL.Data;

public partial class BugtrackerContext : DbContext
{
    private readonly IUserIdProvider _userIdProvider;
    public BugtrackerContext(DbContextOptions<BugtrackerContext> options, IUserIdProvider userIdProvider) : base(options)
    {
        _userIdProvider = userIdProvider;
    }

    // public BugtrackerContext(DbContextOptions<BugtrackerContext> options) : base(options) { }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<CommentAttachment> CommentAttachments { get; set; }

    public virtual DbSet<UploadedFile> Files { get; set; }

    public virtual DbSet<Moderator> Moderators { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductMember> ProductMembers { get; set; }

    public virtual DbSet<ProductModerator> ProductModerators { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<ReportAttachment> ReportAttachments { get; set; }

    public virtual DbSet<ReportReproduce> ReportReproduces { get; set; }

    public virtual DbSet<ReportTag> ReportTags { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int userId = _userIdProvider?.UserId ?? 0;

        foreach (var entry in ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Deleted:
                    var deleted = entry.Entity as IDeleteEntity;
                    if (deleted == null) break;
                    deleted.IsDeleted = true;
                    entry.State = EntityState.Modified;
                    break;
                case EntityState.Added:
                    var created = entry.Entity as ICreateEntity;
                    if (created == null) break;
                    created.CreationTime = DateTime.Now;
                    created.CreatorId = userId;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BugtrackerContext).Assembly);
    }
}
