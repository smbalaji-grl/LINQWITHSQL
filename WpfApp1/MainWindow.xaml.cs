using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LinqToSQlDataClassDataContext MergingDataBaseData;
        public MainWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["WpfApp1.Properties.Settings.DemoDBConnectionString"].ConnectionString;

            //string connectionString = ConfigurationManager.ConnectionStrings["WPF_ZooManager.Properties.Settings.DemoDBConnectionString"].ConnectionString;
            MergingDataBaseData = new LinqToSQlDataClassDataContext(connectionString);
            //insertUniversities();
            // insertStudents();
            //insertLecture();
            //insertStudentLectureAssociation();
            getuniversityfromSpecificPerson("balnaji");
        }
        public void insertUniversities()
        {
            MergingDataBaseData.ExecuteCommand("Delete from University");
            University srm = new University();
            srm.Name = "SRM univ";
            MergingDataBaseData.Universities.InsertOnSubmit(srm);
            University VIT = new University() { Name = "VIT univ" };
            MergingDataBaseData.Universities.InsertOnSubmit(VIT);
            MergingDataBaseData.SubmitChanges();
            MainDataGrid.ItemsSource = MergingDataBaseData.Universities;
        }
        public void insertStudents()
        {
            University srm = MergingDataBaseData.Universities.First(x => x.Name.Equals("SRM univ"));
            University vit = MergingDataBaseData.Universities.First(x => x.Name.Equals("VIT univ"));

            List<Student> students = new List<Student>();
            students.Add(new Student() { Name = "Arun", Gender = "male", Universityid = srm.Id });
            students.Add(new Student() { Name = "balaji", Gender = "male", Universityid = vit.Id });
            students.Add(new Student() { Name = "chander", Gender = "male", Universityid = srm.Id });
            students.Add(new Student() { Name = "dinesh", Gender = "male", Universityid = vit.Id });
            students.Add(new Student() { Name = "elango", Gender = "male", Universityid = srm.Id });
            students.Add(new Student() { Name = "frank", Gender = "male", Universityid = vit.Id });
            students.Add(new Student() { Name = "ganesh", Gender = "male", Universityid = srm.Id });
            students.Add(new Student() { Name = "hansa", Gender = "Female", Universityid = vit.Id });

            MergingDataBaseData.Students.InsertAllOnSubmit(students);
            MergingDataBaseData.SubmitChanges();
            MainDataGrid.ItemsSource = MergingDataBaseData.Students;
        }
        public void insertLecture()
        {
            MergingDataBaseData.Lectures.InsertOnSubmit(new Lecture { Name = "Tamil" });
            MergingDataBaseData.Lectures.InsertOnSubmit(new Lecture { Name = "Math" });
            MergingDataBaseData.SubmitChanges();
            MainDataGrid.ItemsSource = MergingDataBaseData.Lectures;
        }
        public void insertStudentLectureAssociation()
        {
            Lecture tamil = MergingDataBaseData.Lectures.First(x => x.Name.Equals("Tamil"));
            Lecture Math = MergingDataBaseData.Lectures.First(x => x.Name.Equals("Math"));

            IEnumerable<Student> students_ = MergingDataBaseData.Students.Where(x => (x.Id % 2 == 0));

            foreach (Student student in students_)
            {
                StudentLecture sl = new StudentLecture();
                sl.Student = student;
                sl.Lecture = tamil;
                MergingDataBaseData.StudentLectures.InsertOnSubmit(sl);

            }

            foreach (Student student in MergingDataBaseData.Students)
            {
                StudentLecture sl = new StudentLecture();
                sl.Student = student;
                sl.Lecture = Math;
                MergingDataBaseData.StudentLectures.InsertOnSubmit(sl);

            }
            MergingDataBaseData.SubmitChanges();
            MainDataGrid.ItemsSource = MergingDataBaseData.StudentLectures;
        }

        public void getuniversityfromSpecificPerson(string inputName)
        {
            University inputStudentUniversity = (from student in MergingDataBaseData.Students where student.Name.Equals(inputName) select student.University).SingleOrDefault();
            if (inputStudentUniversity != null)
            {
                List<University> universities = new List<University>();
                universities.Add(inputStudentUniversity);
                MainDataGrid.ItemsSource = universities;
            }
        }

    }
}

