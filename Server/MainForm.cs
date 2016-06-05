using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using System.IO;

namespace Server
{
    public partial class MainForm : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        private delegate void MyDelegate();

        Server server = new Server(8, 45000, 45000, "My streaming server");


        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonStartStreaming_Click(object sender, EventArgs e)
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.Start();

            videoSource.NewFrame += videoSource_NewFrame;            
        }

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            var frame = (Bitmap)eventArgs.Frame.Clone();
            pictureBox.Image = frame;

            byte[] msg = ImageToByteArray(frame.Clone());

            server.SendMessage(msg);
        }
        
        private byte[] ImageToByteArray(object imageInObj)
        {
            var imageIn = (Bitmap)imageInObj;
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
    }
}
