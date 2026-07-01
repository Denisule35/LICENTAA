using System.ComponentModel.DataAnnotations;

namespace Modern.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string name {  get; set; }
        public string password { get; set; }

    }
}