using System.Drawing;
using System.Windows.Forms;

namespace BobsExternal
{
    public class MainForm : Form
    {
        private Panel sidebar;
        private Panel content;

        public MainForm()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            Text = "Bob's External";
            Size = new Size(1100, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(25, 25, 25);

            sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = Color.FromArgb(18, 18, 18)
            };

            content = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 30, 30)
            };

            Controls.Add(content);
            Controls.Add(sidebar);

            CreateSidebar();
            CreateDashboard();
        }

        private void CreateSidebar()
        {
            Label logo = new Label
            {
                Text = "BOB'S EXTERNAL",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Width = 220,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };

            sidebar.Controls.Add(logo);

            string[] buttons =
            {
                "Dashboard",
                "Visuals",
                "Performance",
                "Profiles",
                "Settings",
                "About"
            };

            int y = 80;

            foreach (string name in buttons)
            {
                Button btn = new Button
                {
                    Text = name,
                    Width = 190,
                    Height = 40,
                    Left = 15,
                    Top = y,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(35, 35, 35)
                };

                btn.FlatAppearance.BorderSize = 0;

                sidebar.Controls.Add(btn);

                y += 50;
            }
        }

        private void CreateDashboard()
        {
            Label title = new Label
            {
                Text = "Dashboard",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Left = 30,
                Top = 20,
                AutoSize = true
            };

            CheckBox enableFeature = new CheckBox
            {
                Text = "Enable Feature",
                ForeColor = Color.White,
                Left = 35,
                Top = 80,
                Width = 200
            };

            TrackBar opacitySlider = new TrackBar
            {
                Left = 35,
                Top = 130,
                Width = 250,
                Minimum = 0,
                Maximum = 100,
                Value = 75
            };

            ComboBox themeBox = new ComboBox
            {
                Left = 35,
                Top = 210,
                Width = 180
            };

            themeBox.Items.Add("Dark");
            themeBox.Items.Add("Light");
            themeBox.SelectedIndex = 0;

            Button saveButton = new Button
            {
                Text = "Save Settings",
                Left = 35,
                Top = 270,
                Width = 140,
                Height = 35,
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            saveButton.FlatAppearance.BorderSize = 0;

            content.Controls.Add(title);
            content.Controls.Add(enableFeature);
            content.Controls.Add(opacitySlider);
            content.Controls.Add(themeBox);
            content.Controls.Add(saveButton);
        }
    }
}
