using Database.Abstractions;

namespace Database.Models;

public class DepartmentModel: IModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int UserId { get; set; }
    public List<DoctorModel>? Doctors { get; set; }
    
    public DepartmentModel()
    {
        Doctors = new List<DoctorModel>();
    }
    
    public DepartmentModel(int id, string name, int userId)
    {
        Id = id;
        Name = name;
        UserId = userId;
        Doctors = new List<DoctorModel>();
    }
    
    public DepartmentModel(int id, string name, int userId,  List<DoctorModel> doctors)
    {
        Id = id;
        Name = name;
        UserId = userId;
        Doctors = doctors;
    }
}