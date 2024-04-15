using Microsoft.Data.SqlClient;
using SchoolManagementApplication.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
namespace SchoolManagementApplication.ViewModel;

public class StudentViewModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly string _connectionString = "Data Source=COGNINE-L152\\SQLEXPRESS;Initial Catalog=SchoolManagement;Integrated Security=True;Trust Server Certificate=True";
    
    public ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>();
    private Student _studentData;
    public Student StudentData
    {
        get { return _studentData; }
        set
        {
            _studentData = value;
            OnPropertyChanged(nameof(StudentData));
        }
    }

    public ICommand AddCommand { get; }
    public ICommand UpdateCommand { get; }
    public ICommand DeleteCommand { get; }
   
    /// <summary>
    /// constructor to initialize the instance of the studenViewModel.
    /// </summary>
    public StudentViewModel()
    {
        AddCommand = new RelayCommand(AddStudent);
        UpdateCommand = new RelayCommand(UpdateStudent, CanUpdateDelete);
        DeleteCommand = new RelayCommand(DeleteStudent, CanUpdateDelete);
        LoadData();
    }

    /// <summary>
    /// Method to fetch the data from the database.
    /// </summary>
    private void LoadData()    
    {
        Students.Clear();
        using SqlConnection connection = new(_connectionString);
        string query = "SELECT * FROM Students";
        SqlCommand command = new(query, connection);
        connection.Open();
        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Students.Add(new Student
            {
                StudentId = (int)reader["StudentId"],
                StudentName = reader["StudentName"].ToString(),
                Age = (int)reader["Age"],
                Gender=reader["Gender"].ToString()
            });
        }

    }

    /// <summary>
    /// Method to check whether there the requries field holds the data to update or delete.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>

    private bool CanUpdateDelete(object obj)
    {
        return _studentData != null;
    }

    /// <summary>
    /// method to insert new data into the database.
    /// </summary>
    /// <param name="obj"></param>

    private void AddStudent(object obj)
    {
        var studentForm = new StudentForm();
        studentForm.SaveClicked += (name,age,gender) =>
        {
            using (SqlConnection connection = new (_connectionString))
            {
                string query = "INSERT INTO Students (StudentName, Age,Gender) VALUES (@StudentName, @Age,@Gender)";
                SqlCommand command = new (query, connection);
                command.Parameters.AddWithValue("@StudentName", name);
                command.Parameters.AddWithValue("@Age", age);
                command.Parameters.AddWithValue("@Gender", gender);
                connection.Open();
                command.ExecuteNonQuery();
            }
            LoadData();
        };
        // creating a window/ dialog box to display the insert form  
        Window window = new ()
        {
           Title = "Add Student",
           Content = studentForm,
           SizeToContent = SizeToContent.WidthAndHeight
        };
        window.ShowDialog();
    }

    /// <summary>
    /// method to update the data of the selected data 
    /// </summary>
    /// <param name="obj"></param>
    private void UpdateStudent(object obj)
    {
        var studentForm = new StudentForm();

        studentForm.SaveClicked += (name, age,gender) =>
        {
            if (_studentData != null)
            {
                using (SqlConnection connection = new (_connectionString))
                {
                    string query = "UPDATE Students SET StudentName = @StudentName, Age = @Age,Gender=@Gender WHERE StudentId = @StudentId";
                    SqlCommand command = new (query, connection);
                    command.Parameters.AddWithValue("@StudentName", name);
                    command.Parameters.AddWithValue("@Age", age);
                    command.Parameters.AddWithValue("@Gender", gender);
                    command.Parameters.AddWithValue("@StudentId",_studentData.StudentId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                LoadData();
            }

        };

        //binding the details from view to display on the window
        studentForm.txtStudentName.Text = StudentData.StudentName;
        studentForm.txtAge.Text = StudentData.Age.ToString();
        studentForm.txtGender.Text=StudentData.Gender;
        Window window = new ()
        {
            Title = "Update Employee",
            Content = studentForm,
            SizeToContent = SizeToContent.WidthAndHeight
        };

        window.ShowDialog();
    }

    /// <summary>
    /// method to delete the data based on the student id.
    /// </summary>
    /// <param name="obj"></param>
    private void DeleteStudent(object obj)
    {
        if (StudentData!= null)
        {
            using (SqlConnection connection = new (_connectionString))
            {
                string query = "DELETE FROM Students WHERE StudentId = @StudentId";
                SqlCommand command = new (query, connection);
                command.Parameters.AddWithValue("@StudentId", StudentData.StudentId);
                connection.Open();
                command.ExecuteNonQuery();
            }
            LoadData();
        }

    }

    /// <summary>
    /// method to notify the subscribers of property change.
    /// </summary>
    /// <param name="propertyName"></param>

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
