namespace DemoHisaab2.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Hisaab : DbContext
    {
        public Hisaab()
            : base("name=Hisaab")
        {
        }

        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<User_Accounts> User_Accounts { get; set; }
        public virtual DbSet<User_Details> User_Details { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .Property(e => e.CreditAccount)
                .IsUnicode(false);

            modelBuilder.Entity<Transaction>()
                .Property(e => e.DebitAccount)
                .IsUnicode(false);

            modelBuilder.Entity<Transaction>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<User_Accounts>()
                .Property(e => e.AccountName)
                .IsUnicode(false);

            modelBuilder.Entity<User_Accounts>()
                .Property(e => e.AccountType)
                .IsUnicode(false);

            modelBuilder.Entity<User_Details>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User_Details>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User_Details>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<User_Details>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<User_Details>()
                .Property(e => e.ContactNumber)
                .HasPrecision(10, 0);

            modelBuilder.Entity<User_Details>()
                .HasMany(e => e.Transactions)
                .WithRequired(e => e.User_Details)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User_Details>()
                .HasMany(e => e.User_Accounts)
                .WithRequired(e => e.User_Details)
                .WillCascadeOnDelete(false);
        }
    }
}
