using Database.Abstractions;

namespace Database.Models;

public class PatientModel : IModel
{
    public int Id { get; set; }
    public List<string>? Patients { get; set; }
    public int ? Count { get; set; }
    
    public PatientModel()
    {
        Patients = new List<string>();
    }
    
    public PatientModel(int id, List<string> patients)
    {
        Id = id;
        Patients = patients;
    }
    
    public PatientModel(int id, List<string> patients, int count)
    {
        Id = id;
        Patients = patients;
        Count = count;
    }
}