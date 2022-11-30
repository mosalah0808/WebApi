using System.ComponentModel.DataAnnotations;
using WebApi.Abstractions;

namespace WebApi.Models
{
    public class Customer:BaseEntity
    {
        
        
        [Required]
        public string Firstname { get; init; }

        [Required]
        public string Lastname { get; init; }
    }
}