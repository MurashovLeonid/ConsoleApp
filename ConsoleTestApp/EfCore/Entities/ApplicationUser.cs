using System;
using System.ComponentModel.DataAnnotations;

namespace ConsoleTestApp.EfCore.Entities
{
    public sealed class ApplicationUser
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhotoUri { get; set; }
        public ApplicationUserPhoto UserPhoto { get; set; }
    }
}
