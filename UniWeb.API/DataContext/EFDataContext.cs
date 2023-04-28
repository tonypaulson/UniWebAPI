using UniWeb.API.Entities;
using UniWeb.API.Enums;
using Carewell.API.SeedDatas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UniWeb.API.DataContext
{
    public class EFDataContext : DbContext
    {
        private IConfiguration _config;

        public EFDataContext(DbContextOptions<EFDataContext> options,
        IConfiguration configuration) : base(options)
        {
            _config = configuration;
        }

        public DbSet<CurrencyUnit> CurrencyUnits { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admin { get; set; }
        
        public DbSet<UserToken> UserToken { get; set; }
        public DbSet<AdminToken> AdminToken { get; set; }
        
        public DbSet<Profile> Profile { get; set; }
        public DbSet<TenantAdmin> TenantAdmins { get; set; }
        public DbSet<TemporaryPassword> TemporaryPasswords { get; set; }
        
        public DbSet<MailRecord> MailRecords { get; set; }
         

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            var currencyUnits = modelBuilder.Entity<CurrencyUnit>();

            var tenant = modelBuilder.Entity<Admin>();     
            var token = modelBuilder.Entity<AdminToken>();

            currencyUnits.HasKey(x => x.Id);
            currencyUnits.Property(x => x.Country).HasMaxLength(150).IsRequired(true);
            currencyUnits.Property(x => x.Code).HasMaxLength(50).IsRequired(true);
            currencyUnits.Property(x => x.Currency).HasMaxLength(50).IsRequired(true);
            currencyUnits.Property(x => x.Symbol).HasMaxLength(50).IsRequired(true);      

            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<User>()
                .Property(x => x.FirstName)
                .HasMaxLength(200)
                .IsRequired(true);
            modelBuilder.Entity<User>()
                .Property(x => x.LastName)
                .HasMaxLength(200)
                .IsRequired(true);
            modelBuilder.Entity<User>()
                .Property(x => x.Email)
                .HasMaxLength(190)
                .IsRequired(true);
            modelBuilder.Entity<User>()
                .Property(x => x.MobileNumber)
                .HasMaxLength(20)
                .IsRequired(true);
            modelBuilder.Entity<User>()
                .Property(x => x.Password)
                .HasMaxLength(60);
            //.IsRequired(true);
            modelBuilder.Entity<User>()
                .Property(x => x.Gender)
                .IsRequired(true);
            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email);
            

           
            modelBuilder.Entity<User>().HasData(new SeedAdminUser().Seed());

            modelBuilder.Entity<Admin>()
            .HasKey(p => p.Id);
            modelBuilder.Entity<Admin>().HasData(new SeedAdmin().Seed());
           

            token.HasKey(s => s.Id);
            token.HasOne(o => o.Admin).WithMany(m => m.Tokens).HasForeignKey(f => f.AdminId).IsRequired(true);
            token.Property(p => p.Value).IsRequired(true);
            token.Property(p => p.CreatedDate).IsRequired(true);
            token.Property(p => p.UpdatedDate).IsRequired(true);
            token.Property(p => p.ExpiryTime).IsRequired(true);

            modelBuilder.Entity<TenantAdmin>()
            .HasKey(x => new { x.TenantId, x.UserId });
            modelBuilder.Entity<TenantAdmin>()
            .HasData(new TenantAdmin[]
            {
                new TenantAdmin()
                {
                    TenantId = 1,
                    UserId = 1
                }
            });

            modelBuilder.Entity<TemporaryPassword>()
            .HasKey(x => x.Id);
            modelBuilder.Entity<TemporaryPassword>()
            .Property(x => x.Token)
            .IsRequired(true)
            .HasMaxLength(100);
            modelBuilder.Entity<TemporaryPassword>()
            .Property(x => x.Password)
            .IsRequired(true)
            .HasMaxLength(8);
            modelBuilder.Entity<TemporaryPassword>()
            .HasOne(x => x.User)
            .WithOne(x => x.TemporaryPassword)
            .HasForeignKey<TemporaryPassword>(x => x.UserId);
            modelBuilder.Entity<TemporaryPassword>()
            .HasAlternateKey(x => x.Token);

            modelBuilder.Entity<User>()
            .HasAlternateKey(x => x.Email);        
            
           

          

            modelBuilder.Entity<MailRecord>()
            .HasKey(x => x.Id);
            modelBuilder.Entity<MailRecord>()
            .Property(x => x.Subject)
            .IsRequired(true)
            .HasMaxLength(200);
            modelBuilder.Entity<MailRecord>()
            .Property(x => x.To)
            .IsRequired(true)
            .HasMaxLength(2000);
            modelBuilder.Entity<MailRecord>()
            .Property(x => x.Body)
            .IsRequired(true);
            modelBuilder.Entity<MailRecord>()
            .Property(x => x.Status)
            .IsRequired(true);
            modelBuilder.Entity<MailRecord>()
            .Property(x => x.Error)
            .IsRequired(false);
            modelBuilder.Entity<MailRecord>()
            .Property(x => x.CreatedAt)
            .IsRequired(true);
            modelBuilder.Entity<MailRecord>()
            .Property(x => x.UpdatedAt)
            .IsRequired(true);
        }
    }
}
