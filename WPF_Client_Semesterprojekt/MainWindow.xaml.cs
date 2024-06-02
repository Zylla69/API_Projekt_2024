using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

namespace WPF_Client_Semesterprojekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string apiUrl = "http://localhost:8080/api";
        List<Notiz> notes = new List<Notiz>();

        public MainWindow()
        {

            InitializeComponent();
            Inhalt.Text = "Notiz Inhalt";
            Ueberschrift.Text = "Notiz Überschrift";
            LoadNotes();
        }

        private void speichern_Click(object sender, RoutedEventArgs e)
        {
            if (Ueberschrift == null || Ueberschrift.Equals(""))
            {
                MessageBox.Show("Inhalt oder Überschrift darf nicht leer sein.");
            }
            else if (Inhalt == null || Inhalt.Equals(""))
            {
                MessageBox.Show("Inhalt oder Überschrift darf nicht leer sein.");
            }
            else if (notizlist.SelectedItem != null)
            {
                UpdateNote();
            }
            else
            {
                AddNote();
            }

            Inhalt.Text = "Notiz Inhalt";
            Ueberschrift.Text = "Notiz Überschrift";
        }

        private void loeschen_Click(object sender, RoutedEventArgs e)
        {
            DeleteNote();
            Inhalt.Text = "Notiz Inhalt";
            Ueberschrift.Text = "Notiz Überschrift";
        }

        private async void LoadNotes()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(apiUrl + "/notizen");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                notes.Clear();
                notizlist.Items.Clear();
                notes = JsonSerializer.Deserialize<List<Notiz>>(responseBody);

                foreach (Notiz note in notes)
                {
                    ListBoxItem listBoxItem = new ListBoxItem();
                    listBoxItem.Content = note.title;
                    listBoxItem.Tag = note;
                    notizlist.Items.Add(listBoxItem);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error beim laden der Notizen: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AddNote()
        {
            try
            {
                Notiz newNote = new Notiz { title = Ueberschrift.Text, text = Inhalt.Text };
                HttpClient client = new HttpClient();
                string json = JsonSerializer.Serialize(newNote);
                HttpResponseMessage response = await client.PostAsync(apiUrl + "/notiz", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
                MessageBox.Show("Notiz erfolgreich hinzugefügt.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadNotes();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error beim hinzufügen der Notiz: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void UpdateNote()
        {
            try
            {
                ListBoxItem selectedListBoxItem = (ListBoxItem)notizlist.SelectedItem;
                Notiz selectedNote = (Notiz)selectedListBoxItem.Tag;
                selectedNote.title = Ueberschrift.Text;
                selectedNote.text = Inhalt.Text;
                string apiUrlWithId = $"{apiUrl + "/notiz"}";
                HttpClient client = new HttpClient();
                string json = JsonSerializer.Serialize(selectedNote);
                HttpResponseMessage response = await client.PutAsync(apiUrlWithId, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
                MessageBox.Show("Notiz erfolgreich aktualisiert.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadNotes();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Wähle eine Notiz aus zum aktualisieren.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error beim aktualisieren der Notiz: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeleteNote()
        {
            try
            {
                ListBoxItem selectedListBoxItem = (ListBoxItem)notizlist.SelectedItem;
                Notiz selectedNote = (Notiz)selectedListBoxItem.Tag;
                string apiUrlWithId = $"{apiUrl + "/notiz"}/{selectedNote.id}";
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync(apiUrlWithId);
                response.EnsureSuccessStatusCode();

                LoadNotes();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Wähle eine Notiz aus zum Löschen.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error beim löschen der Notiz: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Ueberschrift_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Ueberschrift.Text == "Notiz Überschrift")
            {
                Ueberschrift.Text = "";
            }
        }

        private void Inhalt_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Inhalt.Text == "Notiz Inhalt")
            {
                Inhalt.Text = "";
            }
        }

        private void Ueberschrift_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Ueberschrift.Text))
            {
                Ueberschrift.Text = "Notiz Überschrift";
            }
        }

        private void Inhalt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Inhalt.Text))
            {
                Inhalt.Text = "Notiz Inhalt";
            }
        }

        private void ShowNoteDetails()
        {
            try
            {
                ListBoxItem selectedListBoxItem = (ListBoxItem)notizlist.SelectedItem;
                if (selectedListBoxItem != null)
                {
                    Notiz selectedNote = (Notiz)selectedListBoxItem.Tag;
                    Inhalt.Text = selectedNote.text;
                    Ueberschrift.Text = selectedNote.title;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying note details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void notizlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowNoteDetails();
        }

        private void abbrechen_Click(object sender, RoutedEventArgs e)
        {
            notizlist.SelectedItem = null;
            Inhalt.Text = "Notiz Inhalt";
            Ueberschrift.Text = "Notiz Überschrift";
        }
    }
}