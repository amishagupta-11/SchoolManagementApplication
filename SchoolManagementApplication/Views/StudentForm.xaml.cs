using System.Windows;
using System.Windows.Controls;

namespace SchoolManagementApplication
{
    public partial class StudentForm : UserControl
    {
        public event Action<string,int,string>? SaveClicked;

        public StudentForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// method to save the data on clicking the save button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string name = txtStudentName.Text;
            if (int.TryParse(txtAge.Text, out int age))
            {
                // Age successfully parsed, proceed with the logic
                string gender = txtGender.Text;
                SaveClicked?.Invoke(name, age, gender);

                // Close the window to hide the popup
                Window.GetWindow(this).Close();
            }
            else
            {
                // Handle the case where the age is not a valid integer
                MessageBox.Show("Please enter a valid age.");
            }
        }

        /// <summary>
        /// method to perform cancellation call on clicking the cancel button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

    }
}