using Database.Abstractions;

namespace Database.Models;

public class Department: IModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime ? CreatedAt { get; set; }
    public List<DoctorModel>? Doctors { get; set; }
    
    public Department()
    {
        Doctors = new List<DoctorModel>();
    }
    
    public Department(int id, string name, DateTime createdAt)
    {
        Id = id;
        Name = name;
        CreatedAt = createdAt;
        Doctors = new List<DoctorModel>();
    }
    
    public Department(int id, string name, DateTime createdAt, List<DoctorModel> doctors)
    {
        Id = id;
        Name = name;
        CreatedAt = createdAt;
        Doctors = doctors;
    }
}