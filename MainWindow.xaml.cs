using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Text.RegularExpressions;
using System.Speech.Synthesis;

namespace WpfApp_Alytalo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Lights olohuone = new Lights();
        Lights keittio = new Lights();
        Thermostat talo = new Thermostat();
        Sauna kiuas = new Sauna();
        public DispatcherTimer SaunaLampoAjastin1 = new DispatcherTimer();
        public DispatcherTimer SaunaLampoAjastin2 = new DispatcherTimer();
        public SpeechSynthesizer synth = new SpeechSynthesizer();
        List<Status> TalonTila = new List<Status>();
        public MainWindow()
        {
            InitializeComponent();
            AsetaDgKentat();

            SaunaLampoAjastin1.Tick += SaunaLampoAjastin1_Tick;
            SaunaLampoAjastin1.Interval = TimeSpan.FromMilliseconds(500);
            SaunaLampoAjastin2.Tick += SaunaLampoAjastin2_Tick;
            SaunaLampoAjastin2.Interval = TimeSpan.FromMilliseconds(500);
        }

        //Talon lämpötila
        private void btnTempSet_Click(object sender, RoutedEventArgs e)
        {
            if (IsNumeric(txtTempSet.Text) && (txtTempSet.Text != ""))
            {
                talo.TempNow = int.Parse(txtTempSet.Text);
                txtTempNow.Text = (talo.TempNow.ToString() + "°");
                txtTempNow.Background = Brushes.LightPink;
                txtTempSet.Text = "";
                synth.Speak("House temperature set at " + talo.TempNow + " degrees.");
                SaunaLampomittari.MinValue = talo.TempNow;
                SaunaLampomittari.MaxValue = kiuas.saunaTemp;
            }
            else
            {
                synth.Speak("Please enter temperature as numbers.");
                txtTempSet.Text = "";
                txtTempSet.Focus();
            }
        }
        public bool IsNumeric(string input)
        {
            return Regex.IsMatch(input, "^[0-9]+$");
        }

        //Valot, olohuone ja keittiö
        private void btnValoOn_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btnValo1On)
            {
                olohuone.LightsOn();
                if (olohuone.On == true)
                {
                    txtValo1.Text = "ON";
                    txtValo1.Background = Brushes.Gold;
                    sldValo1.IsEnabled = true;
                    synth.Speak("Living room lights on.");
                }
            }
            else if (sender == btnValo2On)
            {
                keittio.LightsOn();
                if (keittio.On == true)
                {
                    txtValo2.Text = "ON";
                    txtValo2.Background = Brushes.Gold;
                    sldValo2.IsEnabled = true;
                    synth.Speak("Kitchen lights on.");
                }
            }
        }
        private void btnValoOff_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btnValo1Off)
            {
                olohuone.LightsOff();
                if (olohuone.On == false)
                {
                    txtValo1.Text = "OFF";
                    txtValo1.Background = Brushes.Transparent;
                    sldValo1.IsEnabled = false;
                    txtValo1.IsEnabled = false;
                    sldValo1.Value = 50;
                    synth.Speak("Living room lights off.");
                }
            }
            else if (sender == btnValo2Off)
            {
                keittio.LightsOff();
                if (keittio.On == false)
                {
                    txtValo2.Text = "OFF";
                    txtValo2.Background = Brushes.Transparent;
                    sldValo2.IsEnabled = false;
                    txtValo2.IsEnabled = false;
                    sldValo2.Value = 50;
                    synth.Speak("Kitchen lights off.");
                }
            }
        }
        private void SldValo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender == sldValo1 && olohuone.On == true)
            {
                olohuone.Lumen = sldValo1.Value;
                txtValo1.Text = olohuone.Lumen.ToString();
                if (olohuone.Lumen == 50)
                {
                    txtValo1.Background = Brushes.Gold;
                }
                else if (olohuone.Lumen < 50)
                {
                    txtValo1.Background = Brushes.BlanchedAlmond;
                }
                else if (olohuone.Lumen > 50)
                {
                    txtValo1.Background = Brushes.Yellow;
                }
            }
            else if (sender == sldValo2 && keittio.On == true)
            {
                keittio.Lumen = sldValo2.Value;
                txtValo2.Text = keittio.Lumen.ToString();
                if (keittio.Lumen == 50)
                {
                    txtValo2.Background = Brushes.Gold;
                }
                else if (keittio.Lumen < 50)
                {
                    txtValo2.Background = Brushes.BlanchedAlmond;
                }
                else if (keittio.Lumen > 50)
                {
                    txtValo2.Background = Brushes.Yellow;
                }
            }
        }

        //Sauna
        private void btnTempSetKiuas_Click(object sender, RoutedEventArgs e)
        {
            if (IsNumeric(txtKiuasTempSet.Text) && (txtKiuasTempSet.Text != ""))
            {
                kiuas.saunaTemp = double.Parse(txtKiuasTempSet.Text);
                txtKiuasTempSet.Text = "";
                synth.Speak("Sauna temperature set at " + kiuas.saunaTemp + " degrees.");
                SaunaLampomittari.MaxValue = kiuas.saunaTemp;
                kiuas.saunaInitialValue = SaunaLampomittari.MinValue;
            }
            else
            {
                synth.Speak("Please enter temperature as numbers.");
                txtKiuasTempSet.Text = "";
                txtKiuasTempSet.Focus();
            }
        }
        private void btnKiuas_Click(object sender, RoutedEventArgs e)
        {


            if (sender == btnKiuasOn)
            {
                kiuas.Switched = true;
                txtKiuas.Text = "PÄÄLLÄ";
                txtKiuas.Background = Brushes.Red;
                SaunaLampomittari.CurrentValue = kiuas.saunaInitialValue;
                if (!SaunaLampoAjastin1.IsEnabled)
                {
                    SaunaLampoAjastin1.Start();
                }
                kiuas.Lammita();
                kiuas.saunaCurrentValue = SaunaLampomittari.CurrentValue;
            }
            if (sender == btnKiuasOff)
            {
                kiuas.Switched = false;
                txtKiuas.Text = "";
                txtKiuas.Background = Brushes.Transparent;
                SaunaLampomittari.CurrentValue = kiuas.saunaCurrentValue;
                SaunaLampoAjastin1.Stop();
                if (!SaunaLampoAjastin2.IsEnabled)
                {
                    SaunaLampoAjastin2.Start();
                }
                kiuas.Jaahdyta();
                kiuas.saunaRecedingValue = SaunaLampomittari.CurrentValue;
                kiuas.saunaInitialValue = kiuas.saunaRecedingValue;
            }
        }
        private void SaunaLampoAjastin1_Tick(object sender, EventArgs e)
        {
            if (kiuas.Switched == true)
            {
                btnKiuasOn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else
            {
                SaunaLampoAjastin1.Stop();
            }
        }
        private void SaunaLampoAjastin2_Tick(object sender, EventArgs e)
        {
            if (kiuas.Switched == false)
            {
                btnKiuasOff.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else
            {
                SaunaLampoAjastin2.Stop();
            }
        }

        //Laitteiden tila
        public void btnStatuspdate_Click(object sender, RoutedEventArgs e)
        {
            Status tila = new Status();
            tila.lampotila = txtTempNow.Text;

            dgStatus.Items.Clear();

            if (olohuone.On == true)
            {
                tila.olohuoneValot = "Päällä";
                if (txtValo1.Text == "ON")
                {
                    tila.kirkkausOlohuone = "50";
                }
                else
                {
                    tila.kirkkausOlohuone = txtValo1.Text;
                }
            } else
            {
                tila.olohuoneValot = "Pois";
                tila.kirkkausOlohuone = " ";
            }
            if (keittio.On == true)
            {
                tila.keittioValot = "Päällä";
                if (txtValo2.Text == "ON")
                {
                    tila.kirkkausKeittio = "50";
                }
                else
                {
                    tila.kirkkausKeittio = txtValo2.Text;
                }
            }
            else
            {
                tila.keittioValot = "Pois";
                tila.kirkkausKeittio = " ";
            }
            tila.saunaAsetettuLampotila = kiuas.saunaTemp.ToString();
            if (kiuas.Switched == true)
            {
                tila.sauna = "Päällä";
                tila.saunaLampotila = kiuas.saunaCurrentValue.ToString();

            } else
            {
                tila.sauna = "Pois";
            }
            string currentSaunaLampotila = " ";
            if (kiuas.Switched == false)
            {
                tila.currentSaunaLampotila = kiuas.saunaRecedingValue.ToString();
            } else
            {
                tila.currentSaunaLampotila = tila.saunaLampotila;
            }

            TalonTila.Add(tila);
            dgStatus.Items.Add(tila);

        }

        //Laitteiden tilan, datagrid
        public void AsetaDgKentat()
        {
            DataGridTextColumn textColLampotila = new DataGridTextColumn();
            DataGridTextColumn textColOlohuone = new DataGridTextColumn();
            DataGridTextColumn textColOlohuoneKirkkaus = new DataGridTextColumn();
            DataGridTextColumn textColKeittio = new DataGridTextColumn();
            DataGridTextColumn textColKeittioKirkkaus = new DataGridTextColumn();
            DataGridTextColumn textColSauna = new DataGridTextColumn();
            DataGridTextColumn textColSaunaSet = new DataGridTextColumn();
            DataGridTextColumn textColSaunaTemp = new DataGridTextColumn();

            textColLampotila.Binding = new Binding("lampotila");
            textColOlohuone.Binding = new Binding("olohuoneValot");
            textColOlohuoneKirkkaus.Binding = new Binding("kirkkausOlohuone");
            textColKeittio.Binding = new Binding("keittioValot");
            textColKeittioKirkkaus.Binding = new Binding("kirkkausKeittio");
            textColSauna.Binding = new Binding("sauna");
            textColSaunaSet.Binding = new Binding("saunaAsetettuLampotila");
            textColSaunaTemp.Binding = new Binding("currentSaunaLampotila");

            textColLampotila.Header = "Lämpötila";
            textColOlohuone.Header = "Olohuone valot";
            textColOlohuoneKirkkaus.Header = "Kirkkaus";
            textColKeittio.Header = "Keittiö valot";
            textColKeittioKirkkaus.Header = "Kirkkaus";
            textColSauna.Header = "Sauna";
            textColSaunaSet.Header = "Saunan asetettu lämpötila";
            textColSaunaTemp.Header = "Saunan lämpötila";

            dgStatus.Columns.Add(textColLampotila);
            dgStatus.Columns.Add(textColOlohuone);
            dgStatus.Columns.Add(textColOlohuoneKirkkaus);
            dgStatus.Columns.Add(textColKeittio);
            dgStatus.Columns.Add(textColKeittioKirkkaus);
            dgStatus.Columns.Add(textColSauna);
            dgStatus.Columns.Add(textColSaunaSet);
            dgStatus.Columns.Add(textColSaunaTemp);
        }
    }
}
