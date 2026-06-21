using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Drawing;

namespace BobsExternal
{
    public class WeatherDashboard : Form
    {
        private TextBox cityInput;
        private Button searchButton;
        private Label temperatureLabel;
        private Label weatherLabel;
        private Label humidityLabel;
        private Label windSpeedLabel;
        private Label cityNameLabel;
        private Label statusLabel;
        private const string API_URL = "https://api.open-meteo.com/v1/forecast";
        private const string GEOCODING_URL = "https://geocoding-api.open-meteo.com/v1/search";

        public WeatherDashboard()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            Text = "Weather Dashboard";
            Size = new Size(600, 500);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(25, 25, 25);
            Font = new Font("Segoe UI", 10);

            // Title
            Label title = new Label
            {
                Text = "Weather Dashboard",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Left = 20,
                Top = 15,
                AutoSize = true
            };
            Controls.Add(title);

            // City input
            Label cityLabel = new Label
            {
                Text = "Enter City:",
                ForeColor = Color.White,
                Left = 20,
                Top = 60,
                AutoSize = true
            };
            Controls.Add(cityLabel);

            cityInput = new TextBox
            {
                Left = 20,
                Top = 85,
                Width = 350,
                Height = 35,
                BackColor = Color.FromArgb(35, 35, 35),
                ForeColor = Color.White,
                Text = "New York"
            };
            Controls.Add(cityInput);

            // Search button
            searchButton = new Button
            {
                Text = "Search",
                Left = 380,
                Top = 85,
                Width = 100,
                Height = 35,
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            searchButton.FlatAppearance.BorderSize = 0;
            searchButton.Click += SearchButton_Click;
            Controls.Add(searchButton);

            // Status label
            statusLabel = new Label
            {
                Text = "Ready",
                ForeColor = Color.LimeGreen,
                Left = 20,
                Top = 130,
                AutoSize = true
            };
            Controls.Add(statusLabel);

            // City name
            cityNameLabel = new Label
            {
                Text = "",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Left = 20,
                Top = 160,
                AutoSize = true
            };
            Controls.Add(cityNameLabel);

            // Temperature
            temperatureLabel = new Label
            {
                Text = "Temperature: --°C",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12),
                Left = 20,
                Top = 200,
                AutoSize = true
            };
            Controls.Add(temperatureLabel);

            // Weather description
            weatherLabel = new Label
            {
                Text = "Weather: --",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12),
                Left = 20,
                Top = 240,
                AutoSize = true
            };
            Controls.Add(weatherLabel);

            // Humidity
            humidityLabel = new Label
            {
                Text = "Humidity: --%",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12),
                Left = 20,
                Top = 280,
                AutoSize = true
            };
            Controls.Add(humidityLabel);

            // Wind speed
            windSpeedLabel = new Label
            {
                Text = "Wind Speed: -- km/h",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12),
                Left = 20,
                Top = 320,
                AutoSize = true
            };
            Controls.Add(windSpeedLabel);

            // Load default weather on startup
            LoadWeather("New York");
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            string city = cityInput.Text.Trim();
            if (string.IsNullOrEmpty(city))
            {
                statusLabel.Text = "Please enter a city name";
                statusLabel.ForeColor = Color.Red;
                return;
            }

            await LoadWeather(city);
        }

        private async Task LoadWeather(string city)
        {
            try
            {
                statusLabel.Text = "Loading...";
                statusLabel.ForeColor = Color.Yellow;
                searchButton.Enabled = false;

                // Geocode the city to get coordinates
                string geoUrl = $"{GEOCODING_URL}?name={city}&count=1&language=en&format=json";
                using (HttpClient client = new HttpClient())
                {
                    var geoResponse = await client.GetAsync(geoUrl);
                    if (!geoResponse.IsSuccessStatusCode)
                    {
                        throw new Exception("City not found");
                    }

                    string geoContent = await geoResponse.Content.ReadAsStringAsync();
                    JObject geoData = JObject.Parse(geoContent);

                    if (geoData["results"] == null || geoData["results"].Count() == 0)
                    {
                        throw new Exception("City not found");
                    }

                    double latitude = (double)geoData["results"][0]["latitude"];
                    double longitude = (double)geoData["results"][0]["longitude"];
                    string cityName = (string)geoData["results"][0]["name"];
                    string country = (string)geoData["results"][0]["country"];

                    // Fetch weather data
                    string weatherUrl = $"{API_URL}?latitude={latitude}&longitude={longitude}&current=temperature_2m,relative_humidity_2m,weather_code,wind_speed_10m&temperature_unit=celsius";
                    var weatherResponse = await client.GetAsync(weatherUrl);
                    weatherResponse.EnsureSuccessStatusCode();

                    string weatherContent = await weatherResponse.Content.ReadAsStringAsync();
                    JObject weatherData = JObject.Parse(weatherContent);

                    // Extract data
                    JObject current = (JObject)weatherData["current"];
                    double temperature = (double)current["temperature_2m"];
                    int humidity = (int)current["relative_humidity_2m"];
                    double windSpeed = (double)current["wind_speed_10m"];
                    int weatherCode = (int)current["weather_code"];

                    // Update UI
                    cityNameLabel.Text = $"{cityName}, {country}";
                    temperatureLabel.Text = $"Temperature: {temperature}°C";
                    humidityLabel.Text = $"Humidity: {humidity}%";
                    windSpeedLabel.Text = $"Wind Speed: {windSpeed} km/h";
                    weatherLabel.Text = $"Weather: {GetWeatherDescription(weatherCode)}";
                    statusLabel.Text = "✓ Weather loaded successfully";
                    statusLabel.ForeColor = Color.LimeGreen;
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Error: {ex.Message}";
                statusLabel.ForeColor = Color.Red;
            }
            finally
            {
                searchButton.Enabled = true;
            }
        }

        private string GetWeatherDescription(int code)
        {
            return code switch
            {
                0 => "Clear sky",
                1 or 2 => "Partly cloudy",
                3 => "Overcast",
                45 or 48 => "Foggy",
                51 or 53 or 55 => "Light rain",
                61 or 63 or 65 => "Rain",
                71 or 73 or 75 => "Snow",
                80 or 82 => "Showers",
                85 or 86 => "Snow showers",
                95 or 96 or 99 => "Thunderstorm",
                _ => "Unknown"
            };
        }
    }
}
