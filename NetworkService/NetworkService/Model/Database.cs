using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public class DataBase   //baza podataka
    {
        public static Dictionary<int, ElectricityMeter> ElectricityMeters { get; set; } = new Dictionary<int, ElectricityMeter>();    //svi ElectricityMeteri
        public static Dictionary<string, ElectricityMeter> CanvasElectricityMeters { get; set; } = new Dictionary<string, ElectricityMeter>();    //ElectricityMeteri prevuceni u monitoring
        public static ObservableCollection<ElectricityMeter> DataGridElectricityMeters { get; set; } = new ObservableCollection<ElectricityMeter>(); //serveri u tabeli
    }

}
