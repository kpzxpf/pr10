using System.ComponentModel.DataAnnotations;

namespace pr10.models;

public class Service
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
}