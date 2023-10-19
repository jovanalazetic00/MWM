using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkService.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {

        //private BindableBase _currentViewModel;
        //private NetworkEntitiesViewModel _networkEntitiesViewModel = new NetworkEntitiesViewModel();
        public MyICommand TabCommand1 { get; }
        public MyICommand TabCommand2 { get; }
        public MyICommand TabCommand3 { get; }


        private int count = 10; // Inicijalna vrednost broja objekata u sistemu
                                // ######### ZAMENITI stvarnim brojem elemenata
                                //           zavisno od broja entiteta u listi
        private int index;  //promenljive koje ce dobiti vrednosti nakon parsiranja pristigle poruke sa mettering sim aplikacije
        private int id;
        private double value;
        private string path = Environment.CurrentDirectory + "\\LogFile.txt";   //putanja do fajla gde ce biti upisivane vrednosti pristigle sa metering sim
                                                                                //fajl ce biti u debug folderu
        private NetworkEntitiesViewModel networkEntitiesViewModel = new NetworkEntitiesViewModel();     //instnce svih viewModela zaduzenih za funkcionalnost viewa
        private NetworkDisplayViewModel networkDisplayViewModel = new NetworkDisplayViewModel();
        private MeasurementGraphViewModel measurementGraphViewModel = new MeasurementGraphViewModel();
        private BindableBase currentViewModel;  //trenutni view

        public MyICommand TerminalCommand { get; set; }     //komanda povezana na terminal (sva povezivanja su preko binding-a)

        public MainWindowViewModel()
        {

            createListener(); //Povezivanje sa sim aplikacijom

            currentViewModel = networkEntitiesViewModel;
            TabCommand1 = new MyICommand(OnChangeViewNetworkEntities);
            TabCommand2 = new MyICommand(OnChangeViewNetworkDisplay);
            TabCommand3 = new MyICommand(OnChangeViewMeasurementGraph);


        }

        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);
            }
        }
        private void OnChangeViewNetworkEntities()
        {
            currentViewModel = networkEntitiesViewModel;
            OnPropertyChanged("CurrentViewModel");
        }
        private void OnChangeViewNetworkDisplay()
        {
            networkEntitiesViewModel.NapuniBazu();

            currentViewModel = networkDisplayViewModel;
            OnPropertyChanged("CurrentViewModel");
        }

        private void OnChangeViewMeasurementGraph()
        {
            networkEntitiesViewModel.NapuniBazu();

            currentViewModel = measurementGraphViewModel;
            OnPropertyChanged("CurrentViewModel");
        }

        private void createListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25565);
            tcp.Start();

            var listeningThread = new Thread(() =>
            {
                while (true)
                {
                    var tcpClient = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(param =>
                    {
                        //Prijem poruke
                        NetworkStream stream = tcpClient.GetStream();
                        string incomming;
                        byte[] bytes = new byte[1024];
                        int i = stream.Read(bytes, 0, bytes.Length);
                        //Primljena poruka je sacuvana u incomming stringu
                        incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        //Ukoliko je primljena poruka pitanje koliko objekata ima u sistemu -> odgovor
                        if (incomming.Equals("Need object count"))
                        {
                            //Response
                            /* Umesto sto se ovde salje count.ToString(), potrebno je poslati 
                             * duzinu liste koja sadrzi sve objekte pod monitoringom, odnosno
                             * njihov ukupan broj (NE BROJATI OD NULE, VEC POSLATI UKUPAN BROJ)
                             * */
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(DataBase.ElectricityMeters.Count().ToString());
                            stream.Write(data, 0, data.Length);
                        }
                        else
                        {

                            //U suprotnom, ElectricityMeter je poslao promenu stanja nekog objekta u sistemu
                            Console.WriteLine(incomming); //Na primer: "Entitet_1:272"

                            //################ IMPLEMENTACIJA ####################
                            // Obraditi poruku kako bi se dobile informacije o izmeni
                            // Azuriranje potrebnih stvari u aplikaciji
                            string[] split = incomming.Split('_', ':'); //parsiranje poruke i njen upis u fajl
                            index = Int32.Parse(split[1]);
                            value = Double.Parse(split[2]);
                            networkEntitiesViewModel.NapuniBazu();

                            DataBase.ElectricityMeters.ElementAt(index).Value.Value = value;  //azuriranje pristigle vrednosti
                            id = DataBase.ElectricityMeters.ElementAt(index).Value.Id;
                            if (NetworkEntitiesViewModel.ElectricityMeters.Count == DataBase.ElectricityMeters.Values.Count())
                            {
                                NetworkEntitiesViewModel.ElectricityMeters.FirstOrDefault(x => x.Id == id).Value = value;
                            }
                            DataBase.DataGridElectricityMeters[index].Value = value;
                            Save(); //upis u fajl


                        }
                    }, null);
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }

        private void Save()
        {
            StreamWriter writer;
            using (writer = new StreamWriter(path, true))
            {
                writer.WriteLine("Id=" + id + "_Value=" + value + "_Time=" + DateTime.Now);
            }
        }

    }

}
