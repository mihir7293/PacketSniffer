using Be.Windows.Forms;
using Newtonsoft.Json;
using PacketDotNet;
using SharpPcap;
using SharpPcap.Npcap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace PacketSniffer
{
    public partial class PacketSniffer : Form
    {
        /// <summary>
        /// When true the background thread will terminate
        /// </summary>
        /// <param name="args">
        /// A <see cref="string"/>
        /// </param>
        private bool BackgroundThreadStop;

        /// <summary>
        /// Object that is used to prevent two threads from accessing
        /// PacketQueue at the same time
        /// </summary>
        /// <param name="args">
        /// A <see cref="string"/>
        /// </param>
        private object QueueLock = new object();

        /// <summary>
        /// The queue that the callback thread puts packets in. Accessed by
        /// the background thread when QueueLock is held
        /// </summary>
        private List<RawCapture> PacketQueue = new List<RawCapture>();

        private ICaptureDevice device;
        private Queue<A3Packet> packetStrings;
        private List<A3Packet> packetData;
        private BindingSource bs;
        private Thread backgroundThread;
        private PacketArrivalEventHandler arrivalEventHandler;
        private CaptureStoppedEventHandler captureStoppedEventHandler;

        Config config;
        Crypt crypt;
        public PacketSniffer()
        {
            InitializeComponent();
            bgLoadDeviceList.DoWork += BgLoadDeviceList_DoWork;
            bgLoadDeviceList.RunWorkerCompleted += BgLoadDeviceList_RunWorkerCompleted;
            this.MinimumSize = new Size(800, 500);
        }

        private void BgLoadDeviceList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblStatus.Text = "Loaded Interfaces Successfully !!!";
        }

        private void BgLoadDeviceList_DoWork(object sender, DoWorkEventArgs e)
        {
            lblStatus.Text = "Loading Interfaces ....";
            LoadInterfaces();
        }

        private void CaptureForm_Load(object sender, EventArgs e)
        {
            config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("Config.json"));
            crypt = new Crypt();
            bgLoadDeviceList.RunWorkerAsync();
        }

        private void LoadInterfaces()
        {
            if (cmbDeviceList.InvokeRequired)
            {
                this.Invoke(new Action(LoadInterfaces));
            }
            else
            {
                cmbDeviceList.Items.Clear();
                foreach (NpcapDevice device in CaptureDeviceList.Instance)
                {
                    if (device.Loopback)
                    {
                        cmbDeviceList.Items.Add("LoopBack");
                    }
                    else
                    {
                        cmbDeviceList.Items.Add(device.Interface.FriendlyName != null ? device.Interface.FriendlyName : device.Interface.Description);
                    }
                }
                cmbDeviceList.SelectedIndex = 0;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            device = CaptureDeviceList.Instance[cmbDeviceList.SelectedIndex];
            device.Open();
            device.Filter = "tcp and (port " + config.LoginAgentPort + " or port " + config.ZoneAgentPort + ") and host " + config.ServerIp;
            packetStrings = new Queue<A3Packet>();
            packetData = new List<A3Packet>();
            
            bs = new BindingSource();
            dataGridPackets.DataSource = bs;

            BackgroundThreadStop = false;
            backgroundThread = new Thread(BackgroundThread);
            backgroundThread.Start();

            arrivalEventHandler = new PacketArrivalEventHandler(device_OnPacketArrival);
            device.OnPacketArrival += arrivalEventHandler;
            captureStoppedEventHandler = new CaptureStoppedEventHandler(device_OnCaptureStopped);
            device.OnCaptureStopped += captureStoppedEventHandler;

            device.Open();

            // start the background capture
            device.StartCapture();

            btnStart.Enabled = false;
            btnStop.Enabled = true;
            lblStatus.Text = "Capturing packets started ...";
            this.Text = this.Text + "*";
        }

        /// <summary>
        /// Checks for queued packets. If any exist it locks the QueueLock, saves a
        /// reference of the current queue for itself, puts a new queue back into
        /// place into PacketQueue and unlocks QueueLock. This is a minimal amount of
        /// work done while the queue is locked.
        ///
        /// The background thread can then process queue that it saved without holding
        /// the queue lock.
        /// </summary>
        private void BackgroundThread()
        {
            while (!BackgroundThreadStop)
            {
                bool shouldSleep = true;

                lock (QueueLock)
                {
                    if (PacketQueue.Count != 0)
                    {
                        shouldSleep = false;
                    }
                }

                if (shouldSleep)
                {
                    Thread.Sleep(250);
                }
                else // should process the queue
                {
                    List<RawCapture> ourQueue;
                    lock (QueueLock)
                    {
                        // swap queues, giving the capture callback a new one
                        ourQueue = PacketQueue;
                        PacketQueue = new List<RawCapture>();
                    }

                    foreach (var packet in ourQueue)
                    {
                        // Here is where we can process our packets freely without
                        // holding off packet capture.
                        //
                        // NOTE: If the incoming packet rate is greater than
                        //       the packet processing rate these queues will grow
                        //       to enormous sizes. Packets should be dropped in these
                        //       cases

                        var packetWrapper = new A3Packet(packet, config, crypt);
                        if (config.ShowUnique && packetStrings.Where(x => x.Length == packetWrapper.Length).Count() == 0)
                        {

                            this.BeginInvoke(new MethodInvoker(delegate
                            {
                                packetStrings.Enqueue(packetWrapper);
                                packetData.Add(packetWrapper);
                            }
                            ));
                        }
                        else if (!config.ShowUnique)
                        {
                            this.BeginInvoke(new MethodInvoker(delegate
                            {
                                packetStrings.Enqueue(packetWrapper);
                                packetData.Add(packetWrapper);
                            }
                            ));
                        }

                    }

                    this.BeginInvoke(new MethodInvoker(delegate
                    {
                        bs.DataSource = packetStrings.Reverse();
                    }
                    ));
                }
            }
        }
        void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {

            // lock QueueLock to prevent multiple threads accessing PacketQueue at
            // the same time

            Packet packet = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
            var tcpPacket = (TcpPacket)packet.PayloadPacket.PayloadPacket;
            if (tcpPacket.PayloadData.Length > 9)
            {
                lock (QueueLock)
                {
                    PacketQueue.Add(e.Packet);
                }
            }
        }
        void device_OnCaptureStopped(object sender, CaptureStoppedEventStatus status)
        {
            if (status != CaptureStoppedEventStatus.CompletedWithoutError)
            {
                MessageBox.Show("Error stopping capture", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Shutdown()
        {
            if (device != null)
            {
                device.StopCapture();
                device.Close();
                device.OnPacketArrival -= arrivalEventHandler;
                device.OnCaptureStopped -= captureStoppedEventHandler;
                device = null;

                // ask the background thread to shut down
                BackgroundThreadStop = true;

                // wait for the background thread to terminate
                backgroundThread.Join();

                btnStop.Enabled = false;
                btnStart.Enabled = true;
            }
        }

        private void CaptureForm_Resize(object sender, EventArgs e)
        {
            dataGridPackets.Size = new Size(this.Width - 41, dataGridPackets.Size.Height);
            hexbData.Size = new Size(this.Width - 41, status.Location.Y - hexbData.Location.Y - 3);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Shutdown();
            lblStatus.Text = "Capturing packets stopped ...";
            this.Text = this.Text.TrimEnd('*');
        }

        private void CaptureForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Shutdown();
        }

        private void dataGridPackets_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            hexbData.ByteProvider = new DynamicByteProvider(packetData[packetData.Count - 1 - e.RowIndex].Data);
            hexbData.Refresh();
        }

        private void dataGridPackets_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridPackets.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }
    }
}
