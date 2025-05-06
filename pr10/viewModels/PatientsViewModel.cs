using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using pr10.models;

namespace pr10.viewModels;

public class PatientsViewModel : INotifyPropertyChanged
{
    private readonly ClinicContext _db = new();
    public ObservableCollection<Patient> Patients { get; }
    public ObservableCollection<Doctor> Doctors { get; }
    public ICollectionView View { get; }

    private Patient _selected;

    public Patient Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            OnPropertyChanged();
        }
    }

    private string _search;

    public string Search
    {
        get => _search;
        set
        {
            _search = value;
            OnPropertyChanged();
            View.Refresh();
        }
    }

    private Doctor _filterDoc;

    public Doctor FilterDoc
    {
        get => _filterDoc;
        set
        {
            _filterDoc = value;
            OnPropertyChanged();
            View.Refresh();
        }
    }

    public ICommand AddCmd { get; }
    public ICommand DeleteCmd { get; }

    public event PropertyChangedEventHandler PropertyChanged;

    void OnPropertyChanged(string p = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));

    public PatientsViewModel()
    {
        _db.Database.EnsureCreated();
        
        Patients = new ObservableCollection<Patient>(
            _db.Patients
                .Include(p => p.Appointments).ThenInclude(a => a.Doctor)
                .Include(p => p.Appointments).ThenInclude(a => a.Diagnosis)
                .Include(p => p.Appointments).ThenInclude(a => a.Service)
                .ToList());
        Doctors = new ObservableCollection<Doctor>(_db.Doctors.ToList());
        
        View = CollectionViewSource.GetDefaultView(Patients);
        View.Filter = obj =>
        {
            var p = obj as Patient;
            bool okS = string.IsNullOrWhiteSpace(Search)
                       || p.LastName.Contains(Search, StringComparison.OrdinalIgnoreCase);
            bool okD = FilterDoc == null
                       || p.Appointments.Any(a => a.DoctorId == FilterDoc.Id);
            return okS && okD;
        };
        
        AddCmd = new Relay(_ =>
        {
            var p = new Patient { FirstName = "Новый", LastName = "Пациент" };
            _db.Patients.Add(p);
            _db.SaveChanges();
            Patients.Add(p);
        });
        
        DeleteCmd = new Relay(_ =>
        {
            if (Selected == null) return;
            _db.Patients.Remove(Selected);
            _db.SaveChanges();
            Patients.Remove(Selected);
        });
    }

    public void SaveChanges() => _db.SaveChanges();
    
    class Relay : ICommand
    {
        private readonly Action<object> _exec;
        public Relay(Action<object> exec) => _exec = exec;
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object _) => true;
        public void Execute(object param) => _exec(param);
    }
}