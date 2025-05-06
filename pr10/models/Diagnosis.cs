using System.ComponentModel.DataAnnotations;

namespace pr10.models;

public class Diagnosis
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
}