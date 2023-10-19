using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NetworkService.ViewModel
{
    public class NetworkDisplayViewModel : BindableBase
    {
        private ListView lv;
        public BindingList<ElectricityMeter> Items { get; set; } //objekti raspolozivi za dragdrop (monitoring)
        public MyICommand<ListView> MLBUCommand { get; set; }       //sve komande
        public MyICommand<ElectricityMeter> SCCommand { get; set; }
        public MyICommand<Canvas> DCommand { get; set; }
        public MyICommand<Canvas> FreeCommand { get; set; }
        public MyICommand<Canvas> LCommand { get; set; }
        public MyICommand<ListView> LLWCommand { get; set; }
        public MyICommand NVCommand { get; set; }
        public static ElectricityMeter draggedItem = null;
        private bool dragging = false;
        private static bool exists = false;
        private int selectedIndex = 0;


        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        public NetworkDisplayViewModel()
        {
            Items = new BindingList<ElectricityMeter>();     //izdvajanje objekata za monitoring svi iz baze osim onih koji su vec u monitoringu
            foreach (var item in DataBase.ElectricityMeters.Values)
            {
                exists = false;
                foreach (ElectricityMeter ex in DataBase.CanvasElectricityMeters.Values)
                {
                    if (ex.Id == item.Id)
                    {
                        exists = true;
                        break;
                    }
                }

                if (exists == false)
                    Items.Add(new ElectricityMeter(item));
            }
            MLBUCommand = new MyICommand<ListView>(OnMLBU);
            SCCommand = new MyICommand<ElectricityMeter>(SelectionChange);
            DCommand = new MyICommand<Canvas>(OnDrop);
            FreeCommand = new MyICommand<Canvas>(OnFree);
            LCommand = new MyICommand<Canvas>(OnLoad);
            LLWCommand = new MyICommand<ListView>(OnLLW);
        }

        public void OnLLW(ListView listview)
        {
            lv = listview;
        }

        public void OnLoad(Canvas c)
        {   //postavi u canvas
            if (DataBase.CanvasElectricityMeters.ContainsKey(c.Name))
            {
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(DataBase.CanvasElectricityMeters[c.Name].Type.ImgSrc);
                logo.EndInit();
                c.Background = new ImageBrush(logo);
                c.Resources.Add("taken", true);
                CheckValue(c);
            }
        }

        public void OnFree(Canvas c)
        {   //oslobadjanje canvasa
            try
            {
                if (c.Resources["taken"] != null)
                {
                    c.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    ((Border)c.Children[0]).BorderBrush = Brushes.Transparent;

                    foreach (var item in DataBase.ElectricityMeters.Values)
                    {
                        if (!Items.Contains(item) && DataBase.CanvasElectricityMeters[c.Name].Id == item.Id)
                        {
                            Items.Add(new ElectricityMeter(item));
                        }
                    }
                    c.Resources.Remove("taken");
                    DataBase.CanvasElectricityMeters.Remove(c.Name);
                }

            }
            catch (Exception) { }
        }

        public void OnDrop(Canvas c)
        {   //dodato i obrisano iz items
            if (draggedItem != null)
            {
                if (c.Resources["taken"] == null)
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(draggedItem.Type.ImgSrc);
                    logo.EndInit();
                    c.Background = new ImageBrush(logo);
                    DataBase.CanvasElectricityMeters[c.Name] = draggedItem;
                    c.Resources.Add("taken", true);
                    Items.Remove(Items.FirstOrDefault(x => x.Name == draggedItem.Name));
                    SelectedIndex = 0;
                    CheckValue(c);
                }
                dragging = false;
            }
        }

        public void OnMLBU(ListView lw)
        {       //reset nismo prevukli u canvas
            draggedItem = null;
            lw.SelectedItem = null;
            dragging = false;
        }

        public void SelectionChange(ElectricityMeter o)
        {       //belezimo sta smo prevukli
            if (!dragging)
            {
                dragging = true;
                draggedItem = new ElectricityMeter(o);
                DragDrop.DoDragDrop(lv, draggedItem, DragDropEffects.Move);
            }
        }

        private void CheckValue(Canvas c)
        {//pracenje vrednosti
            Dictionary<int, ElectricityMeter> temp = new Dictionary<int, ElectricityMeter>();
            foreach (var ob in DataBase.ElectricityMeters.Values)
            {
                temp.Add(ob.Id, ob);
            }
            Task.Delay(2000).ContinueWith(_ =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (DataBase.CanvasElectricityMeters.Count != 0)
                    {
                        if (DataBase.CanvasElectricityMeters.ContainsKey(c.Name))
                        {
                            if (temp[DataBase.CanvasElectricityMeters[c.Name].Id].Value < 0.34 || temp[DataBase.CanvasElectricityMeters[c.Name].Id].Value > 2.73)
                            {
                                ((Border)(c).Children[0]).BorderBrush = Brushes.Red;        //postavljanje okvira u odnosu na vrednsot
                            }
                            else
                            {
                                ((Border)(c).Children[0]).BorderBrush = Brushes.Transparent;
                            }
                        }
                        else
                        {
                            ((Border)(c).Children[0]).BorderBrush = Brushes.Transparent;
                        }
                    }
                });
                CheckValue(c);
            });
        }
    }
}
