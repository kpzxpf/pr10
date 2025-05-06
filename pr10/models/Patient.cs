using System.ComponentModel.DataAnnotations;

namespace pr10.models;

public class Patient
{
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
}