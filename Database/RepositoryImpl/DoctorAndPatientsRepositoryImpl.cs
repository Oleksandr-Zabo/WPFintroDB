using Core.Repository.DoctorAndPatientsRepository;
            using Database.DBProvider;
            using Database.Models;
            
            namespace Database.Repository;
            
            public class DoctorAndPatientsRepositoryImpl : DoctorAndPatientsRepository<IEnumerable<dynamic>?>
            {
                private readonly DatabaseProvider _databaseProvider;
                public DoctorAndPatientsRepositoryImpl(DatabaseProvider databaseProvider)
                {
                    _databaseProvider = databaseProvider;
                }
            
                public override async Task<IEnumerable<dynamic>?> GetDoctorAndPatientsByUserLogin(string login)
                {
                    var result = await _databaseProvider.GetPatientsAndDoctorByUserLogin(login);
                    if (result == null || !result.Any())
                    {
                        return null;
                    }
            
                    return result;
                }
            
                public override async Task<bool> AddDoctorAndPatientsByUserLogin(string login, string doctor, string patient)
                {
                    var result = await _databaseProvider.InsertDoctorAndPatientByUserLogin(login, doctor, patient);
                    return result > 0;
                }
            }