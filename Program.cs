using System;
using System.Drawing;
using System.Windows.Forms;

namespace BobsExternal
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            
            // Show menu to select which app to run
            using (Form menu = new Form())
            {
                menu.Text = "Bob's External - Select App";
                menu.Size = new Size(400, 300);
                menu.StartPosition = FormStartPosition.CenterScreen;
                menu.BackColor = Color.FromArgb(25, 25, 25);
                menu.FormBorderStyle = FormBorderStyle.FixedDialog;
                menu.MaximizeBox = false;
                menu.MinimizeBox = false;

                Label title = new Label
                {
                    Text = "Select Application",
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    Left = 20,
                    Top = 20,
                    AutoSize = true
                };
                menu.Controls.Add(title);

                Button mainFormBtn = new Button
                {
                    Text = "Main Dashboard",
                    Width = 340,
                    Height = 50,
                    Left = 20,
                    Top = 80,
                    BackColor = Color.FromArgb(0, 120, 215),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 11)
                };
                mainFormBtn.FlatAppearance.BorderSize = 0;
                mainFormBtn.Click += (s, e) =>
                {
                    menu.Close();
                    Application.Run(new MainForm());
                };
                menu.Controls.Add(mainFormBtn);

                Button weatherBtn = new Button
                {
                    Text = "Weather Dashboard",
                    Width = 340,
                    Height = 50,
                    Left = 20,
                    Top = 150,
                    BackColor = Color.FromArgb(0, 180, 100),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 11)
                };
                weatherBtn.FlatAppearance.BorderSize = 0;
                weatherBtn.Click += (s, e) =>
                {
                    menu.Close();
                    Application.Run(new WeatherDashboard());
                };
                menu.Controls.Add(weatherBtn);

                Application.Run(menu);
            }
        }
    }
}
