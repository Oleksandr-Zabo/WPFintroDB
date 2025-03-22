using Database.Abstractions;
        
        namespace Database.Models;
        
        public class DoctorModel : IModel
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public int DepartmentId { get; set; }
            public PatientModel? Patient { get; set; }
        
            public DoctorModel()
            {
                Patient = new PatientModel();
            }
            
            public DoctorModel(int id, string name, int departmentId)
            {
                Id = id;
                Name = name;
                DepartmentId = departmentId;
                Patient = new PatientModel();
            }
        
            public DoctorModel(int id, string name, int departmentId, PatientModel? patient)
            {
                Id = id;
                Name = name;
                DepartmentId = departmentId;
                Patient = patient;
            }
        }