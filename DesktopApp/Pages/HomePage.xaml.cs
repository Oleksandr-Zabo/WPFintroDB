using System.Windows;
using System.Windows.Controls;
using Core.Repository.DoctorAndPatientsRepository;
using Database.DBProvider;
using Database.Models;
using Database.Repository;

namespace DesktopApp.Pages;

public partial class HomePage : UserControl
{
    UserModel _user;
    private DatabaseProvider? _databaseProvider;
    private DepartmentDoctorPatients? _departmentDoctorPatients;
    private DoctorAndPatientsRepository<DepartmentDoctorPatients?>? _doctorAndPatientsRepository;

    public HomePage(UserModel user, DatabaseProvider? databaseProvider)
    {
        _user = user;
        _databaseProvider = databaseProvider;
        _doctorAndPatientsRepository = new DoctorAndPatientsRepositoryImpl(databaseProvider);
        InitializeComponent();
       
    }

    private void OnAddClick(object sender, RoutedEventArgs e)
    {
        
    }
    
    void FillDataGrid(DepartmentDoctorPatients departmentDoctorPatients)
    {
        foreach (var doctor in departmentDoctorPatients.Patients)
        {
            var dataGridCells = new List<DataGridCell>();
            var dataGridCell = new DataGridCell();
            dataGridCell.Content = doctor.Key;
            dataGridCells.Add(dataGridCell);
            dataGridCell = new DataGridCell();
            dataGridCell.Content = doctor.Value;
            dataGridCells.Add(dataGridCell);
            dgMarkAndSubject.Items.Add(dataGridCells);
        }
        dgMarkAndSubject.Items.Refresh();
    }

    private async void HomePage_OnLoaded(object sender, RoutedEventArgs e)
    {
        _departmentDoctorPatients = await _doctorAndPatientsRepository?.GetDoctorAndPatientsByUserLogin(_user.login);
        if (_departmentDoctorPatients != null)
        {
            FillDataGrid(_departmentDoctorPatients);
        }
        
    }
}