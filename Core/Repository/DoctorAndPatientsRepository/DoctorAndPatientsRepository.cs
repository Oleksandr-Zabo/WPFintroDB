namespace Core.Repository.DoctorAndPatientsRepository;

public abstract class DoctorAndPatientsRepository<T>
{
    public abstract Task<T> GetDoctorAndPatientsByUserLogin(string login);
    public abstract Task<bool> AddDoctorAndPatientsByUserLogin(string login, string doctor, string patient);
}