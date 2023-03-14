using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBlog.Data.Entity.Authorizatiton
{
    public class AppUser : IdentityUser<int>
    {
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string FirstName { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string LastName { get; set; }
    }
}
