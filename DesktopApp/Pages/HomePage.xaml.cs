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
    private DoctorAndPatientsRepository<IEnumerable<dynamic>>? _doctorAndPatientsRepository;

    public HomePage(UserModel user, DatabaseProvider? databaseProvider)
    {
        _user = user;
        _databaseProvider = databaseProvider;
        _doctorAndPatientsRepository = new DoctorAndPatientsRepositoryImpl(databaseProvider);
        InitializeComponent();
       
    }

    private async void OnAddClick(object sender, RoutedEventArgs e)
    {
        try
        {
            await _doctorAndPatientsRepository?.AddDoctorAndPatientsByUserLogin(_user.login, tbDoctor.Text, tbPatient.Text);
            await RefreshDataGrid();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "System Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task RefreshDataGrid()
    {
        var items = await _doctorAndPatientsRepository?.GetDoctorAndPatientsByUserLogin(_user.login);
        dgDoctorAndPatient.ItemsSource = items;
        dgDoctorAndPatient.UpdateLayout();
    }

    private async void HomePage_OnLoaded(object sender, RoutedEventArgs e)
    {
        try
        {
            await RefreshDataGrid();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "System Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}