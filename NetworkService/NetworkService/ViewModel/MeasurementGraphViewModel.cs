using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.ViewModel
{
    public class MeasurementGraphViewModel : BindableBase
    {
        public MyICommand TerminalCommand { get; set; }
        private string path = Environment.CurrentDirectory + "\\LogFile.txt";
        public static ObservableCollection<ElectricityMeter> List { get; } = new ObservableCollection<ElectricityMeter>();
        private ElectricityMeter _selectedListItem = new ElectricityMeter();
        public MyICommand ShowGraph { get; set; }


        private string h1;          //vrednosti
        private string h2;
        private string h3;
        private string h4;
        private string h5;
        private string c1;  //boje
        private string c2;
        private string c3;
        private string c4;
        private string c5;
        private string d1;          //datumi
        private string d2;
        private string d3;
        private string d4;
        private string d5;
        public MeasurementGraphViewModel()
        {
            ShowGraph = new MyICommand(ShowGraphFunc);
            GetEntity();
        }
        public ElectricityMeter SelectedListItem
        {
            get => _selectedListItem;
            set => _selectedListItem = value;
        }

        public string H1
        {
            get { return h1; }
            set
            {
                h1 = value;
                OnPropertyChanged("H1");
            }
        }
        public string H2
        {
            get { return h2; }
            set
            {
                h2 = value;
                OnPropertyChanged("H2");
            }
        }
        public string H3
        {
            get { return h3; }
            set
            {
                h3 = value;
                OnPropertyChanged("H3");
            }
        }
        public string H4
        {
            get { return h4; }
            set
            {
                h4 = value;
                OnPropertyChanged("H4");
            }
        }
        public string H5
        {
            get { return h5; }
            set
            {
                h5 = value;
                OnPropertyChanged("H5");
            }
        }

        public string C1
        {
            get { return c1; }
            set
            {
                c1 = value;
                OnPropertyChanged("C1");
            }
        }

        public string C2
        {
            get { return c2; }
            set
            {
                c2 = value;
                OnPropertyChanged("C2");
            }
        }

        public string C3
        {
            get { return c3; }
            set
            {
                c3 = value;
                OnPropertyChanged("C3");
            }
        }

        public string C4
        {
            get { return c4; }
            set
            {
                c4 = value;
                OnPropertyChanged("C4");
            }
        }

        public string C5
        {
            get { return c5; }
            set
            {
                c5 = value;
                OnPropertyChanged("C5");
            }
        }
        public string D1
        {
            get { return d1; }
            set
            {
                d1 = value;
                OnPropertyChanged("D1");
            }
        }
        public string D2
        {
            get { return d2; }
            set
            {
                d2 = value;
                OnPropertyChanged("D2");
            }
        }
        public string D3
        {
            get { return d3; }
            set
            {
                d3 = value;
                OnPropertyChanged("D3");
            }
        }
        public string D4
        {
            get { return d4; }
            set
            {
                d4 = value;
                OnPropertyChanged("D4");
            }
        }
        public string D5
        {
            get { return d5; }
            set
            {
                d5 = value;
                OnPropertyChanged("D5");
            }
        }

        private void ShowGraphFunc()
        {
            if (SelectedListItem!=null)
            {
                if (DataBase.ElectricityMeters.ContainsKey(SelectedListItem.Id))
                {
                    Show(SelectedListItem.Id);
                }
            }
        }
        private void Show(int id)
        {
            List<double> values = new List<double>();
            List<DateTime> times = new List<DateTime>();

            try
            {
                using (StreamReader sr = new StreamReader(path))    //ocitavanje fajla 
                {
                    List<List<string>> fileData = new List<List<string>>();
                    string textFile = sr.ReadToEnd();
                    string[] str_split = textFile.Split('=','_','\n');
                    int length = str_split.Length;
                    for (int i = 1; i < length; i = i + 6)  //izdvajanje svih id i value
                    {
                        fileData.Add(new List<string> { str_split[i], str_split[i + 2], str_split[i + 4] });
                    }

                    for (int i = 0; i < fileData.Count; i++)    //izdvajanje vrednosti za izabrani id
                    {
                        if (id == int.Parse(fileData[i][0]))
                        {
                            values.Add(double.Parse(fileData[i][1]));
                            times.Add(DateTime.Parse(fileData[i][2]));
                        }
                    }


                    int len = values.Count;
                    if (len > 5)   //ako ih ima vise od 5 obrisi da ostane 5 poslednjih
                    {
                        for (int i = 0; i < len - 5; i++)
                        {
                            values.RemoveAt(0);
                            times.RemoveAt(0);
                        }
                    }
                    else
                    {   //ako ima manje dodaj do 5 poslednju vrednost
                        for (int i = len; i < 5; i++)
                        {
                            values.Add(values[len - 1]);
                            times.Add(times[len - 1]);
                        }
                    }
                }
            }
            catch
            {
                clearGraph();
            }

            if (values.Count == 5)
            {   //skaliranje na kanvas
                H1 = (50 * values[0]).ToString();
                C1 = GetColor(values[0]);
                D1 = times[0].ToString("dd.MM.yyyy. hh:mm");
                H2 = (50 * values[1]).ToString();
                C2 = GetColor(values[1]);
                D2 = times[1].ToString("dd.MM.yyyy. hh:mm");
                H3 = (50 * values[2]).ToString();
                C3 = GetColor(values[2]);
                D3 = times[2].ToString("dd.MM.yyyy. hh:mm");
                H4 = (50 * values[3]).ToString();
                C4 = GetColor(values[3]);
                D4 = times[3].ToString("dd.MM.yyyy. hh:mm");
                H5 = (50 * values[4]).ToString();
                C5 = GetColor(values[4]);
                D5 = times[4].ToString("dd.MM.yyyy. hh:mm");

            }

        }
        private void clearGraph()
        {
            H1 = "0";
            D1 = "";
            H2 = "0";
            D2 = "";
            H3 = "0";
            D3 = "";
            H4 = "0";
            D4 = "";
            H5 = "0";
            D5 = "";

        }
        private string GetColor(double value)
        {
            if (value < 0.34 || value > 2.73)
                return "Red";
            else
                return "Blue";
        }

        private void GetEntity()
        {
            List.Clear();
            foreach (var item in DataBase.ElectricityMeters.Values)
                List.Add(item);
        }
    }
}
