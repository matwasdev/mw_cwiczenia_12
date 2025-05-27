using System.ComponentModel.DataAnnotations;

namespace mw_cwiczenia_12.DTOs;

public class RegisterClientForTripDto
{
    [Required]
    [MaxLength(120)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(120)]
    public string LastName { get; set; }
    
    [Required]
    [MaxLength(120)]
    public string Email { get; set; }
    
    [Required]
    public string Telephone { get; set; }
    
    [Required]
    public string Pesel { get; set; }
    public DateTime? PaymentDate { get; set; }
}