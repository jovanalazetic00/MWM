using NetworkService.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace NetworkService.ViewModel
{
    public class NetworkEntitiesViewModel : BindableBase
    {
        private DataBase dataBase = new DataBase();     //baza 

        public static ObservableCollection<ElectricityMeter> ElectricityMeters { get; } = new ObservableCollection<ElectricityMeter>();
        public ObservableCollection<ElectricityMeter> oldElectricityMeters { get; } = new ObservableCollection<ElectricityMeter>();

        public DataBase DataBase    //svi propertiji. svaki properti se preko bindinga povezuje sa view-om
        { get => dataBase; set => dataBase = value; }

        public List<ElectricityMeter> filter = new List<ElectricityMeter>();
        public static ObservableCollection<Model.Type> ElectricityMeterTypes { get; } = new ObservableCollection<Model.Type>();

        private ElectricityMeter _selectedElectricityMeter = new ElectricityMeter();
        private ElectricityMeter _newElectricityMeter = new ElectricityMeter();
        private Type _typeFilter = new Type();
        private bool _isCheckedLesserThan;
        private bool _isCheckedGreaterThan;
        private int _idFilter;
        private string _validMess;
        public MyICommand AddElectricityMeter { get; set; }
        public MyICommand DeleteElectricityMeter { get; set; }
        public MyICommand FilterElectricityMeter { get; set; }
        public MyICommand UndoFilterElectricityMeter { get; set; }
        public static bool filterActive = false;
        public MyICommand TypeChange { get; set; }

        public ElectricityMeter SelectedElectricityMeter
        {
            get => _selectedElectricityMeter;
            set => _selectedElectricityMeter = value;
        }

        public ElectricityMeter NewElectricityMeter
        {
            get => _newElectricityMeter;
            set => _newElectricityMeter = value;
        }

        public Type TypeFilter
        {
            get => _typeFilter;
            set => _typeFilter = value;
        }

        public bool IsCheckedLesserThan
        {
            get => _isCheckedLesserThan;
            set => _isCheckedLesserThan = value;
        }

        public bool IsCheckedGreaterThan
        {
            get => _isCheckedGreaterThan;
            set => _isCheckedGreaterThan = value;
        }

        public int IdFilter
        {
            get => _idFilter;
            set => _idFilter = value;
        }
        public string ValidMess
        {
            get { return _validMess; }
            set
            {
                _validMess = value;
                OnPropertyChanged("ValidMess");
            }
        }

        public NetworkEntitiesViewModel()
        {
            popuniPodatke();
            AddElectricityMeter = new MyICommand(OnAdd);
            DeleteElectricityMeter = new MyICommand(OnDelete);
            FilterElectricityMeter = new MyICommand(OnFilter);
            UndoFilterElectricityMeter = new MyICommand(UndoFilter);
        }

        private void popuniPodatke()
        {
            if (ElectricityMeters.Count < 1)
            {
                for (int i = 0; i < 10; i++)
                {
                    ElectricityMeter em = new ElectricityMeter();     //dodat jedan objekat u bazu (tabelu)
                    em.Id = i;
                    em.Name = "New Meter " + (i+1);
                    em.Value = 0;
                    if (i % 2 == 0)
                        em.Type = new Model.Type("Interval Meter");
                    else
                        em.Type = new Model.Type("Smart Meter");

                    ElectricityMeters.Add(em);

                }
            }
            if (ElectricityMeterTypes.Count<1)
            {
                ElectricityMeterTypes.Add(new Model.Type("Interval Meter"));
                ElectricityMeterTypes.Add(new Model.Type("Smart Meter"));

            }
        }

        private void OnAdd()
        {
            if(NewElectricityMeter.Id<=0 ||NewElectricityMeter.Name =="" || NewElectricityMeter.Name==null || NewElectricityMeter.Type==null)
            {
                ValidMess = "Popunite obavezna polja. ID mora biti cijeli broj. Name ne smije biti prazan. Tip mora da bude odabran.";
            }
            else if(provjeriPostojiLiID(NewElectricityMeter.Id))
            {
                ValidMess = "Postoji zapis sa unesenim ID-ijem.";
            }
            else
            {
                ElectricityMeters.Add(new ElectricityMeter(NewElectricityMeter.Id, NewElectricityMeter.Name, NewElectricityMeter.Type.Name));
                NewElectricityMeter.Id = 0;
                NewElectricityMeter.Name = null;
                NewElectricityMeter.Type = null;
                OnPropertyChanged("NewElectricityMeter");
                ValidMess = "";
            }

            OnPropertyChanged("ElectricityMeters");

        }

        private void OnDelete()
        {
            if (provjeriMonitoring(SelectedElectricityMeter.Id))
            {
                ValidMess = "Nije moguce obrisati oznaceni element, element je pod monitoringom.";
            }
            else
            {
                ElectricityMeters.Remove(SelectedElectricityMeter);
                ValidMess = "";
            }

            OnPropertyChanged("ElectricityMeters");

        }

        private bool provjeriPostojiLiID(int id)
        {
            bool postoji = false;
            foreach (var item in ElectricityMeters)
                if (item.Id == id)
                    postoji = true;
            return postoji;
        }

        private void OnFilter()
        {
            filterActive = true;
            foreach (var item in ElectricityMeters)   //prolazimo kroz bazu
            {
                if (TypeFilter.Name != null)
                {
                    if (item.Type.Name == TypeFilter.Name)
                        if (IsCheckedLesserThan)  //na osnovu tipa id> id< izdvajamo objekte
                        {
                            if (item.Id < IdFilter)
                                filter.Add(item);
                        }
                        else if (IsCheckedGreaterThan)
                        {
                            if (item.Id > IdFilter)
                                filter.Add(item);
                        }
                        else
                        {
                            filter.Add(item);
                        }
                }
                else
                {
                    if (IsCheckedLesserThan)  //na osnovu tipa id> id< izdvajamo objekte
                    {
                        if (item.Id < IdFilter)
                            filter.Add(item);
                    }
                    else if (IsCheckedGreaterThan)
                    {
                        if (item.Id > IdFilter)
                            filter.Add(item);
                    }
                }


            }
            if (oldElectricityMeters.Count < 1)
                copyList();


            ElectricityMeters.Clear();   //postavljamo ih u datagridservers jer je on povezan na tabelu
            foreach (var item in filter)
            {
                ElectricityMeters.Add(item);
            }
            filter.Clear();
            OnPropertyChanged("ElectricityMeters");

        }
        private void UndoFilter()
        {
            filterActive = false;
            ElectricityMeters.Clear();
            foreach (var item in oldElectricityMeters)
                ElectricityMeters.Add(item);

            OnPropertyChanged("ElectricityMeters");

        }

        private void copyList()
        {
            foreach (var item in ElectricityMeters)
                oldElectricityMeters.Add(item);
        }

        public void NapuniBazu()
        {
            if(!filterActive)
            {
                DataBase.ElectricityMeters.Clear();
                DataBase.DataGridElectricityMeters.Clear();
                foreach (var item in ElectricityMeters)
                {
                    DataBase.ElectricityMeters.Add(item.Id, item);
                    DataBase.DataGridElectricityMeters.Add(item);
                }
            }
        

        }

        private bool provjeriMonitoring(int id)
        {
            bool postoji = false;
            foreach (var item in DataBase.CanvasElectricityMeters.Values)
            {
                if(item.Id==id)
                {
                    postoji = true;
                }
            }
            return postoji;

        }
    }
}
