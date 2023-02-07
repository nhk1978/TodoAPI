using System;
using System.ComponentModel.DataAnnotations.Schema;
using TodoApi.Helpers;

namespace TodoApi.Models
{
    [Table("todo", Schema = "todoapi")]
    public class TodoDTO
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("user_id")]
        public int UserID { get; set; }
        [Column("status")]
        public todo_status Status { get; set; }
    }
}