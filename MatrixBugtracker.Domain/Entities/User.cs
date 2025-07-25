﻿using MatrixBugtracker.Domain.Entities.Base;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.Domain.Entities;

public partial class User : BaseEntity
{
    public bool IsEmailConfirmed { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public string? ModeratorName { get; set; }
    public int? PhotoFileId { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<UploadedFile> UploadedFiles { get; set; } = new List<UploadedFile>();
    public virtual UploadedFile PhotoFile { get; set; }
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    public virtual ICollection<Product> CreatedProducts { get; set; } = new List<Product>();
    public virtual ICollection<ProductMember> JoinedProducts { get; set; } = new List<ProductMember>();
    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public virtual ICollection<Confirmation> Confirmations { get; set; } = new List<Confirmation>();
    public virtual ICollection<UserNotification> Notifications { get; set; } = new List<UserNotification>();
    public virtual ICollection<PlatformNotificationUser> PlatformNotifications { get; set; } = new List<PlatformNotificationUser>();
}
