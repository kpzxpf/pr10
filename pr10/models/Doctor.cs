using System.ComponentModel.DataAnnotations;

namespace pr10.models;

public class Doctor
{
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
}