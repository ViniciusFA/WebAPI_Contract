using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Data
{
    public class DataContext : DbContext
    {
         public DataContext(DbContextOptions<DataContext> options) 
            : base(options)
         {            
         }

         public DbSet<Contract> Contracts { get; set; }
         public DbSet<Installments> Installmentss { get; set; }
    }
}