using Database.Abstractions;
        
        namespace Database.Models;
        
        public class PatientModel : IModel
        {
            public int Id { get; set; }
            public List<string>? Patients { get; set; }
            public int DoctorId { get; set; }
        
            public PatientModel()
            {
                Patients = new List<string>();
            }
        
            public PatientModel(int id, List<string> patients, int doctorId)
            {
                Id = id;
                Patients = patients;
                DoctorId = doctorId;
            }
        }