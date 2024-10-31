using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProductCatalog.Model.Database;
using System.Data;

namespace ProductCatalog.Repository.Common.DbContext
{
    public class DatabaseContext : Microsoft.EntityFrameworkCore.DbContext, IDbContext
    {
        private IDbContextTransaction _currentTransaction;
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<VariantAttribute> VariantAttributes { get; set; }
        public DbSet<VariantAttributeValue> VariantAttributeValues { get; set; }
        public DbSet<ProductVariantAttributeLink> ProductVariantAttributeLinks { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Review> Reviews { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Coupon> CartItems { get; set; }


        public void MarkAsModified(object o, string propertyName)
        {
            Entry(o).Property(propertyName).IsModified = true;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        public async Task<T> ExecuteStoredProcedure<T>(string storedProcedure, params SqlParameter[] parameters)
        {
            await Database.ExecuteSqlRawAsync($"EXEC {storedProcedure}", parameters);
            var outputParam = parameters.FirstOrDefault(p => p.Direction == ParameterDirection.Output);

            if (outputParam?.Value is T result)
            {
                return result;
            }

            throw new InvalidOperationException("The output value is not of the expected type.");
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (_currentTransaction != null)
            {
                return _currentTransaction;
            }

            _currentTransaction = await Database.BeginTransactionAsync(isolationLevel);
            return _currentTransaction;
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync();
                }
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình bảng trung gian ProductVariantAttributeLink (many-to-many relationship)
            modelBuilder.Entity<ProductVariantAttributeLink>()
                .HasKey(pv => new { pv.VariantId, pv.ValueId });  // Thiết lập khóa chính hỗn hợp (Composite Key)

            modelBuilder.Entity<ProductVariantAttributeLink>()
                .HasOne(pv => pv.ProductVariant)
                .WithMany(v => v.ProductVariantAttributeLinks)
                .HasForeignKey(pv => pv.VariantId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete nếu ProductVariant bị xóa

            modelBuilder.Entity<ProductVariantAttributeLink>()
                .HasOne(pv => pv.VariantAttributeValue)
                .WithMany(v => v.ProductVariantAttributeLinks)
                .HasForeignKey(pv => pv.ValueId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete nếu VariantAttributeValue bị xóa

            // Cấu hình cho quan hệ đệ quy (recursive relationship) trong Category
            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);  // Không cho phép xóa nếu còn sub-categories

            // Cấu hình cho các giá trị mặc định nếu cần (optional, đã có trong Data Annotations)
            modelBuilder.Entity<Product>()
                .Property(p => p.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<ProductVariant>()
                .Property(pv => pv.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Category>()
                .Property(c => c.IsActive)
                .HasDefaultValue(true);

            //// Configure the relationship between Cart and Product with no cascade delete
            //modelBuilder.Entity<Cart>()
            //    .HasOne(c => c.Product)
            //    .WithMany(p => p.Carts)
            //    .HasForeignKey(c => c.ProductId)
            //    .OnDelete(DeleteBehavior.Restrict);  // Avoid cascade delete

            //// Configure the relationship between Cart and ProductVariant with no cascade delete
            //modelBuilder.Entity<Cart>()
            //    .HasOne(c => c.ProductVariant)
            //    .WithMany(v => v.Carts)
            //    .HasForeignKey(c => c.VariantId)
            //    .OnDelete(DeleteBehavior.Restrict);  // Avoid cascade delete

            modelBuilder.Entity<Review>()
           .Property(r => r.CreatedAt)
           .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Wishlist>()
                .Property(w => w.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Coupon>()
                .HasIndex(c => c.Code)
                .IsUnique();  // Enforces uniqueness of the coupon code.

            base.OnModelCreating(modelBuilder);  // Gọi base method
        }

    }
}
