using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIService.Model
{
    public class MySqlDbContext : DbContext
    {
        public virtual DbSet<Cadastro> Cadastro { get; set; }
        public virtual DbSet<Header> Header { get; set; }

        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
        {
        }
    }
}
