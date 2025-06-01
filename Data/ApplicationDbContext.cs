using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ExtranetQz.Areas.Usuario.Models;

namespace ExtranetQz.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private static DbContextOptions<ApplicationDbContext> _options;
        public ApplicationDbContext()
            : base(_options)
        {
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            _options = options;
        }
        public DbSet<TUsers> TUsers { get; set; }
    }
}
