namespace pr10.models;

public class Appointment
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    
    public int PatientId { get; set; }
    public Patient Patient { get; set; }

    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    public int DiagnosisId { get; set; }
    public Diagnosis Diagnosis { get; set; }

    public int ServiceId { get; set; }
    public Service Service { get; set; }
}