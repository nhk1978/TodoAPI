using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    [Table("user", Schema = "todoapi")]
    public class User
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("updated")]
        public DateTime Updated { get; set; }
    }
}