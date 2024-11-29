using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp10
{
    public interface ITextAdapter
    {
        string LoadData(string filePath);
        void SaveData(string data, string filePath);
    }

    // Адаптер для Word
    public class WordAdapter : ITextAdapter
    {
        public string LoadData(string filePath)
        {
            // Емуляція завантаження даних з Word
            return $"[Word File] Data loaded from {filePath}";
        }

        public void SaveData(string data, string filePath)
        {
            // Емуляція збереження даних у Word
            File.WriteAllText(filePath, $"[Word File] {data}");
        }
    }

    // Адаптер для Excel
    public class ExcelAdapter : ITextAdapter
    {
        public string LoadData(string filePath)
        {
            // Емуляція завантаження даних з Excel
            return $"[Excel File] Data loaded from {filePath}";
        }

        public void SaveData(string data, string filePath)
        {
            // Емуляція збереження даних у Excel
            File.WriteAllText(filePath, $"[Excel File] {data}");
        }
    }

    // Адаптер для Plain Text
    public class PlainTextAdapter : ITextAdapter
    {
        public string LoadData(string filePath)
        {
            // Завантаження звичайного тексту
            return File.ReadAllText(filePath);
        }

        public void SaveData(string data, string filePath)
        {
            // Збереження звичайного тексту
            File.WriteAllText(filePath, data);
        }
    }

    // Головна форма програми
    public partial class MainForm : Form
    {
        private ITextAdapter _adapter;

        private Label statusLabel;
        private TextBox inputTextBox;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Text Format Adapter";
            this.Width = 600;
            this.Height = 400;

            Button loadWordButton = new Button { Text = "Load Word", Left = 50, Top = 50, Width = 120 };
            loadWordButton.Click += (sender, e) => LoadData(new WordAdapter());
            this.Controls.Add(loadWordButton);

            Button loadExcelButton = new Button { Text = "Load Excel", Left = 200, Top = 50, Width = 120 };
            loadExcelButton.Click += (sender, e) => LoadData(new ExcelAdapter());
            this.Controls.Add(loadExcelButton);

            Button loadPlainTextButton = new Button { Text = "Load Plain Text", Left = 350, Top = 50, Width = 150 };
            loadPlainTextButton.Click += (sender, e) => LoadData(new PlainTextAdapter());
            this.Controls.Add(loadPlainTextButton);

            Button saveButton = new Button { Text = "Save File", Left = 50, Top = 250, Width = 120 };
            saveButton.Click += SaveData;
            this.Controls.Add(saveButton);

            inputTextBox = new TextBox { Left = 50, Top = 100, Width = 500, Height = 100, Multiline = true };
            this.Controls.Add(inputTextBox);

            statusLabel = new Label { Left = 50, Top = 200, Width = 500, Height = 30 };
            this.Controls.Add(statusLabel);
        }

        private void LoadData(ITextAdapter adapter)
        {
            try
            {
                _adapter = adapter;

                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "All Files|*.*"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string data = _adapter.LoadData(openFileDialog.FileName);
                    inputTextBox.Text = data;
                    statusLabel.Text = $"Loaded data from: {openFileDialog.FileName}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        private void SaveData(object sender, EventArgs e)
        {
            try
            {
                if (_adapter == null)
                {
                    MessageBox.Show("No adapter selected. Please load a file first.");
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "All Files|*.*"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _adapter.SaveData(inputTextBox.Text, saveFileDialog.FileName);
                    statusLabel.Text = $"Data saved to: {saveFileDialog.FileName}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}");
            }
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
