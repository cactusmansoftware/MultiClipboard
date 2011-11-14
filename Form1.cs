using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Reflection;
using System.Threading;
using System.Runtime.InteropServices;
using System.Xml;
using System.IO;

namespace MultiClipboard
{
	public partial class Form1 : Form
    {
        #region Instance Variables

        private readonly string fileName = "attributes.xml";

		private IntPtr _ClipboardViewerNext;

		private TextBox[] fields;
		private RadioButton[] radButtons;
        private Button[] lockedButtons;
        private bool[] locked;

		private int swtch;

        #endregion

        #region Properties

        public int Switch {
			get
			{
				return swtch;
			}
			set
			{
                radButtons[value].Checked = true;
			}
		}

        #endregion

        public Form1()
		{
			InitializeComponent();
			InitTextBoxArray();
            InitRadioButtons();
            InitLocked();

            LoadAttributes();

            Switch = 1;

            InitNotifyIcon();

			this.Resize += this.form_Resize;
            this.Shown += this.form_Activate;
            this.FormClosed += this.form_Close;

			_ClipboardViewerNext = SetClipboardViewer(this.Handle);
		}

		public string getClipboardText()
		{
			return fields[Switch].Text;
		}

		protected override void WndProc(ref Message m)
		{
			const int WM_DRAWCLIPBOARD = 0x308;
			const int WM_CHANGECBCHAIN = 0x030D;

			switch (m.Msg)
			{
				case WM_DRAWCLIPBOARD:
					IDataObject obj = Clipboard.GetDataObject();
					if (obj.GetDataPresent(DataFormats.StringFormat) && !this.locked[Switch])
					{
						fields[Switch].Text = Clipboard.GetText();
					}
					break;
				case WM_CHANGECBCHAIN:
					if (m.WParam == _ClipboardViewerNext)
					{
						_ClipboardViewerNext = m.LParam;
					}
					else
					{
						SendMessage(_ClipboardViewerNext, (uint)m.Msg, m.WParam, m.LParam);
					}
					break;
				default:
					base.WndProc(ref m);
					break;
			}
		}

        #region Private Methods

        private void InitTextBoxArray()
		{
			fields = new TextBox[10];
			fields[0] = clip0;
			fields[1] = clip1;
			fields[2] = clip2;
			fields[3] = clip3;
			fields[4] = clip4;
			fields[5] = clip5;
			fields[6] = clip6;
			fields[7] = clip7;
			fields[8] = clip8;
			fields[9] = clip9;
		}

        private void InitRadioButtons()
        {
            radButtons = new RadioButton[10];
            radButtons[0] = this.radioButton0;
            radButtons[1] = this.radioButton1;
            radButtons[2] = this.radioButton2;
            radButtons[3] = this.radioButton3;
            radButtons[4] = this.radioButton4;
            radButtons[5] = this.radioButton5;
            radButtons[6] = this.radioButton6;
            radButtons[7] = this.radioButton7;
            radButtons[8] = this.radioButton8;
            radButtons[9] = this.radioButton9;
            for (int i = 0; i < 10; i++)
            {
                radButtons[i].CheckedChanged += groupBox_CheckChanged;
            }
        }

        private void InitLocked()
        {
            lockedButtons = new Button[10];
            lockedButtons[0] = this.button0;
            lockedButtons[1] = this.button1;
            lockedButtons[2] = this.button2;
            lockedButtons[3] = this.button3;
            lockedButtons[4] = this.button4;
            lockedButtons[5] = this.button5;
            lockedButtons[6] = this.button6;
            lockedButtons[7] = this.button7;
            lockedButtons[8] = this.button8;
            lockedButtons[9] = this.button9;

            locked = new bool[10];
            for (int i = 0; i < 10; i++)
            {
                locked[i] = false;
                lockedButtons[i].Click += button_Click;
            }
        }

        private void InitNotifyIcon()
        {
            ResourceManager rm = new ResourceManager("MultiClipboard.Properties.Resources", Assembly.GetExecutingAssembly());

            notifyIcon1 = new NotifyIcon(this.components);
            notifyIcon1.Icon = (System.Drawing.Icon)rm.GetObject("cactus");
            notifyIcon1.Text = "MutliClipboard";
            notifyIcon1.Visible = true;
            notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);

            ToolStripMenuItem exit = new ToolStripMenuItem();
            exit.Text = "E&xit";
            exit.Image = (Image)rm.GetObject("exit");
            exit.Click += new System.EventHandler(this.menuItem1_Click);

            ToolStripMenuItem save = new ToolStripMenuItem();
            save.Text = "S&ave";
            save.Image = (Image)rm.GetObject("save");
            save.Click += new System.EventHandler(this.menuItem2_Click);

            // Initialize contextMenu1
            this.contextMenuStrip1.Items.AddRange(
                        new System.Windows.Forms.ToolStripItem[] { exit, save });

            notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
        }

        private XmlDocument CreateXMLDoc()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("MultiClipboard");
            doc.AppendChild(root);
            for (int i = 0; i < 10; i++)
            {
                XmlElement row = doc.CreateElement("row");
                row.SetAttribute("id", i.ToString());
                XmlElement data = doc.CreateElement("data");
                data.InnerText = this.fields[i].Text;
                XmlElement locked = doc.CreateElement("locked");
                locked.InnerText = this.locked[i].ToString();
                row.AppendChild(data);
                row.AppendChild(locked);

                root.AppendChild(row);
            }
            return doc;
        }

        private void LoadAttributes()
        {
            XmlDocument doc = null;
            FileStream reader = null;
            if (File.Exists(fileName)) // Don't issue an error if the file just isn't there.
            {
                try
                {
                    doc = new XmlDocument();
                    reader = new FileStream(fileName, FileMode.Open);
                    doc.Load(reader);
                }
                catch (Exception e)
                {
                    doc = null;
                    MessageBox.Show("Error loading saved data.");
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }

            if (doc == null)
                return;

            try
            {
                XmlElement root = doc.DocumentElement;
                for (int i = 0; i < root.ChildNodes.Count; i++)
                {
                    XmlElement row = (XmlElement)root.ChildNodes[i];
                    int index = Int32.Parse(row.GetAttribute("id"));
                    for (int j = 0; j < row.ChildNodes.Count; j++)
                    {
                        XmlElement element = (XmlElement)row.ChildNodes[j];
                        if(element.Name == "data")
                            fields[index].Text = element.InnerText;
                        else if(element.Name == "locked")
                        {
                            bool val = Boolean.Parse(element.InnerText);
                            locked[index] = val;
                            ToggleButtonImage(index);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading saved data.");
            }
        }

        private void ToggleButtonImage(int index)
        {
            if (locked[index])
                lockedButtons[index].BackgroundImage = lockedButtons[index].ImageList.Images[0];
            else
                lockedButtons[index].BackgroundImage = lockedButtons[index].ImageList.Images[1];
            this.fields[index].ReadOnly = locked[index];
        }

        private void SelectField()
        {
            fields[Switch].Select(0, fields[Switch].Text.Length);
        }

        #endregion

        #region Event Handlers

        private void groupBox_CheckChanged(object Sender, EventArgs e)
        {
            RadioButton rad = Sender as RadioButton;
            if (rad != null)
            {
                this.swtch = Int32.Parse(rad.Text.Substring(0, 1));
                SelectField();
            }
        }

		private void notifyIcon1_DoubleClick(object Sender, EventArgs e)
		{
			this.Show();
			WindowState = FormWindowState.Normal;
			this.Activate();
		}

		private void menuItem1_Click(object Sender, EventArgs e)
		{
			// Close the form, which closes the application.
			this.Close();
		}

        private void menuItem2_Click(object Sender, EventArgs e)
        {
            XmlDocument doc = CreateXMLDoc();
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(fileName);
                doc.Save(writer);
            }
            catch (Exception er)
            {
                MessageBox.Show("Error saving state.\n" + er.ToString());
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        private void form_Resize(object sender, EventArgs e)
		{
			if (FormWindowState.Minimized == WindowState)
				this.Hide();
		}

        private void button_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            if (b != null)
            {
                int index = Int32.Parse(b.Name.Substring(6, 1));
                locked[index] = !(locked[index]);
                ToggleButtonImage(index);
            }
        }

        private void form_Activate(object sender, EventArgs e)
        {
            SelectField();
        }

        private void form_Close(object sender, EventArgs e)
        {
            // Since we're not passing this form into the main thread in
            // Program.cs, we need to exit the application manually.
            Application.Exit();
        }

        #endregion

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr LParam, IntPtr WParam);
	}
}
