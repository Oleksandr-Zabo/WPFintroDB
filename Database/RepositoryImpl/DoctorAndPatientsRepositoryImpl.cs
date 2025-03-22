using Core.Repository.DoctorAndPatientsRepository;
            using Database.DBProvider;
            using Database.Models;
            
            namespace Database.Repository;
            
            public class DoctorAndPatientsRepositoryImpl : DoctorAndPatientsRepository<DepartmentDoctorPatients?>
            {
                private readonly DatabaseProvider _databaseProvider;
                public DoctorAndPatientsRepositoryImpl(DatabaseProvider databaseProvider)
                {
                    _databaseProvider = databaseProvider;
                }
            
                public override async Task<DepartmentDoctorPatients?> GetDoctorAndPatientsByUserLogin(string login)
                {
                    var result = await _databaseProvider.GetPatientsByDoctorLogin(login);
                    if (result == null || !result.Any())
                    {
                        return null;
                    }
            
                    var departmentDoctorPatients = new DepartmentDoctorPatients
                    {
                        Department = new DepartmentModel(), // Assuming you need to set the department here
                        Patients = result.GroupBy(p => p.DoctorId)
                                         .ToDictionary(g => new DoctorModel { Id = g.Key }, g => g.ToList())
                    };
            
                    return departmentDoctorPatients;
                }
            
                public override async Task<bool> AddDoctorAndPatientsByUserLogin(string login, string doctor, string patient)
                {
                    var result = await _databaseProvider.InsertDoctorAndPatientByUserLogin(login, patient);
                    return result > 0;
                }
            }