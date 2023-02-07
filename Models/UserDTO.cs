using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    //[Table("user", Schema = "todoapi")]
    public class UserDTO
    {
        [Required]
        public long Id { get; set; }
        [Required]
        //[Column("email")]
        public string Email { get; set; }
        [Required]
        //[Column("password")]
        public string Password { get; set; }
        /*[Column("created")]
        public DateTime Created { get; set; }
        [Column("updated")]
        public DateTime Updated { get; set; }*/
    }
}