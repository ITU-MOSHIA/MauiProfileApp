using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiProfileApp.Views
{
    public partial class ProfilePage : ContentPage
    {
        private readonly string profileFilePath = Path.Combine(FileSystem.AppDataDirectory, "profile.json");

        public ProfilePage()
        {
            InitializeComponent();
            LoadProfile();
        }

        private async void LoadProfile()
        {
            try
            {
                if (File.Exists(profileFilePath))
                {
                    string json = await File.ReadAllTextAsync(profileFilePath);
                    var profile = JsonSerializer.Deserialize<Profile>(json);

                    if (profile != null)
                    {
                        NameEntry.Text = profile.Name;
                        SurnameEntry.Text = profile.Surname;
                        EmailEntry.Text = profile.Email;
                        BioEditor.Text = profile.Bio;

                        if (!string.IsNullOrEmpty(profile.ProfileImagePath) && File.Exists(profile.ProfileImagePath))
                        {
                            ProfileImage.Source = ImageSource.FromFile(profile.ProfileImagePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load profile: {ex.Message}", "OK");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                var profile = new Profile
                {
                    Name = NameEntry.Text?.Trim(),
                    Surname = SurnameEntry.Text?.Trim(),
                    Email = EmailEntry.Text?.Trim(),
                    Bio = BioEditor.Text?.Trim(),
                    ProfileImagePath = ProfileImage.Source is FileImageSource fileSource ? fileSource.File : null
                };

                string json = JsonSerializer.Serialize(profile);
                await File.WriteAllTextAsync(profileFilePath, json);

                // Show a success alert
                await DisplayAlert("Success", "Profile saved successfully!", "OK");

                // Provide visual feedback by temporarily changing button text
                if (sender is Button button)
                {
                    string originalText = button.Text;
                    button.Text = "✔ Saved!";
                    await Task.Delay(1500); // Wait for 1.5 seconds
                    button.Text = originalText;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save profile: {ex.Message}", "OK");
            }
        }

        private async void OnChoosePictureClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Select a profile picture"
                });

                if (result != null)
                {
                    ProfileImage.Source = ImageSource.FromFile(result.FullPath);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to choose picture: {ex.Message}", "OK");
            }
        }
    }

    public class Profile
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string ProfileImagePath { get; set; }
    }
}
