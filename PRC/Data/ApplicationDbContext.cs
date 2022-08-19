using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PRC.Models;
using PRC.Models.ViewModels;

namespace PRC.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Newsletter> Newsletters { get; set; }
        public DbSet<Slideshow> Slideshow { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Widget> Widgets { get; set; }
        public DbSet<Motd> Motd { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Question> Questions { get; set; }
    }
}
