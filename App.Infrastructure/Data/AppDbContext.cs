using App.Domain.Entities;
using App.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Data
{
    public class AppDbContext:IdentityDbContext<AppUser,IdentityRole<int>,int>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Workspace> Workspaces => Set<Workspace>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<BoardColumn> BoardColumns => Set<BoardColumn>();
        public DbSet<TaskItem> TaskItems => Set<TaskItem>();
        public DbSet<SubTask> SubTasks => Set<SubTask>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();


        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            #region Enum mapping
            builder.Entity<TaskItem>()
                .Property(t=>t.Priority)
                .HasConversion(
                    v=>v.ToString(),
                    v=>(TaskPriority)Enum.Parse(typeof(TaskPriority), v)
                );
            #endregion

            #region Workspace Configuration
            builder.Entity<Workspace>()
                .HasOne(w => w.Owner)
                .WithMany(o=> o.OwnedWorkspaces)
                .HasForeignKey(w => w.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Project Configuration
            builder.Entity<Project>()
                .HasOne(p => p.Workspace)
                .WithMany(w => w.Projects)
                .HasForeignKey(p => p.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region BoardColumn Configuration
            builder.Entity<BoardColumn>()
                .HasOne(bc => bc.Project)
                .WithMany(p => p.Columns)
                .HasForeignKey(bc => bc.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region TaskItem Configuration
            builder.Entity<TaskItem>()
                .HasOne(t => t.Column)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.ColumnId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TaskItem>()
                .HasOne(t => t.Assignee)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssigneeId)
                .OnDelete(DeleteBehavior.SetNull);
            #endregion

            #region SubTask Configuration
            builder.Entity<SubTask>()
                .HasOne(s => s.TaskItem)
                .WithMany(t => t.SubTasks)
                .HasForeignKey(s => s.TaskItemId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Comment Configuration
            builder.Entity<Comment>()
                .HasOne(c => c.TaskItem)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region ActivityLog Configuration
            builder.Entity<ActivityLog>()
                 .HasOne(a => a.TaskItem)
                 .WithMany(t => t.ActivityLogs)
                 .HasForeignKey(a => a.TaskItemId)
                 .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ActivityLog>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion


        }
    }
}
