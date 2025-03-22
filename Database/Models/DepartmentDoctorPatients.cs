using Database.Abstractions;

namespace Database.Models;

public class DepartmentDoctorPatients : IModel
{
    public DepartmentModel Department { get; set; }
    public Dictionary<DoctorModel, List<PatientModel>> Patients { get; set; }

    public DepartmentDoctorPatients()
    {
        Department = new DepartmentModel();
        Patients = new Dictionary<DoctorModel, List<PatientModel>>();
    }

    public DepartmentDoctorPatients(DepartmentModel department, Dictionary<DoctorModel, List<PatientModel>> patients)
    {
        Department = department;
        Patients = patients;
    }
}