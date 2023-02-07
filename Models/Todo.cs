using System.ComponentModel.DataAnnotations.Schema;
using System;
using TodoApi.Helpers;

namespace TodoApi.Models
{   

    [Table("todo", Schema = "todoapi")]
    public class Todo
    {        
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("user_id")]
        public long UserID { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("updated")]
        public DateTime Updated { get; set; }
        [Column("status")]
        public todo_status Status { get; set; }
    }
}