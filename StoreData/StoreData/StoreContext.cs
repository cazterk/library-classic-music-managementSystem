using Microsoft.EntityFrameworkCore;
using StoreData.Models;

namespace StoreData
{
    public class StoreContext : DbContext
    {

        public StoreContext()
        {

        }

        public StoreContext(DbContextOptions options) : base(options) { }

        public DbSet<Music> Music { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }
        public DbSet<CheckoutHistory> CheckoutHistories { get; set; }
        public DbSet<StoreBranch> StoreBranches { get; set; }
        public DbSet<BranchHours> BranchHours { get; set; }
        public DbSet<StoreCard> StoreCards { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<StoreAsset> StoreAssets { get; set; }
        public DbSet<Hold> Holds { get; set; }
    }
}
 