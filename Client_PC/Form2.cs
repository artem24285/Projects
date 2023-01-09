using AForge.Video.DirectShow;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Client_PC
{
    public partial class Form2 : Form
    {
        FilterInfoCollection VideoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        UdpClient udpClient = new UdpClient();
        WebClient Client = new WebClient();
        public static Bitmap BM = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {//Стандартный размер (627,455)           
            MinimumSize = new Size(400, 400);
            pictureBox1.MinimumSize = new Size(385, 385);
            

            if (VideoDevices.Count > 0)
            {
                foreach (FilterInfo Device in VideoDevices)
                {
                    toolStripComboBox_Choice.Items.Add(Device.Name);
                }
                toolStripComboBox_Choice.SelectedIndex = 0;
                if (VideoDevices.Count == 1)
                {
                    pictureBox1.Size = new Size(627, 455);
                }
                else if (VideoDevices.Count == 2)
                {
                    pictureBox1.Size = new Size(627, 455);
                }
            }

        }

        private async void toolStripButtonStart_Click(object sender, EventArgs e)
        {
            var Port = int.Parse(ConfigurationManager.AppSettings.Get("Port"));
            var Client = new UdpClient(Port);
            while (true)
            {
                var data = await Client.ReceiveAsync();
                using (var Ms = new MemoryStream(data.Buffer))
                {
                    pictureBox1.Image = new Bitmap(Ms);
                }
                label_Counter.Text = $"Счётчик байтов: {data.Buffer.Length * sizeof(byte)}";
            }
        }

        private void Form2_DoubleClick(object sender, EventArgs e)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            MessageBox.Show(string.Join("\n", host.AddressList.Where(i => i.AddressFamily == AddressFamily.InterNetwork).Select(i => i.ToString())));
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            pictureBox1= null;
        }
    }
}
