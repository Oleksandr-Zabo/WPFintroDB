using Database.Abstractions;

namespace Database.Models;

public class DoctorModel: IModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public PatientModel? Patient { get; set; }
    
    public DoctorModel()
    {
        Patient = new PatientModel();
    }
    
    public DoctorModel(int id, string name)
    {
        Id = id;
        Name = name;
        Patient = new PatientModel();
    }
    
    public DoctorModel(int id, string name, PatientModel patient)
    {
        Id = id;
        Name = name;
        Patient = patient;
    }
}