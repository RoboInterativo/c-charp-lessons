using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace TaskManagerGUI
{
    // Task management class (equivalent to Tasc_Act1 in C++)
    public class Task
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public double DueDate { get; set; }

        public void AddTask(string name, string description, string status, double dueDate)
        {
            this.Name = name;
            this.Description = description;
            this.Status = status;
            this.DueDate = dueDate;

            try
            {
                using (StreamWriter writer = new StreamWriter(name + ".txt"))
                {
                    writer.WriteLine($"Task Name: {name}");
                    writer.WriteLine($"Task Description: {description}");
                    writer.WriteLine($"Task Status: {status}");
                    writer.WriteLine($"Task Due Date: {dueDate}");
                }
                MessageBox.Show("Task successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void DeleteTask(string name)
        {
            try
            {
                if (File.Exists(name + ".txt"))
                {
                    File.Delete(name + ".txt");
                    MessageBox.Show("Task successfully deleted!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error: Task not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void PrintTask(string name)
        {
            try
            {
                if (File.Exists(name + ".txt"))
                {
                    string content = File.ReadAllText(name + ".txt");
                    MessageBox.Show(content, "Task Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error: Unable to open task!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error viewing task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void PopulateTaskList(ListBox listBox)
        {
            listBox.Items.Clear();
            try
            {
                var txtFiles = Directory.GetFiles(".", "*.txt").Select(Path.GetFileName);
                foreach (var file in txtFiles)
                {
                    listBox.Items.Add(file);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error populating task list: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // Main form class
    public class TaskManagerForm : Form
    {
        private readonly Task task = new Task();
        private TextBox nameTextBox;
        private TextBox statusTextBox;
        private ListBox taskListBox;
        private Button addButton;
        private Button deleteButton;
        private Button viewButton;

        public TaskManagerForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Form properties
            this.Text = "Task Manager";
            this.Size = new System.Drawing.Size(600, 400);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Add Button
            addButton = new Button
            {
                Text = "Add Task",
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(120, 30)
            };
            addButton.Click += AddButton_Click;
            this.Controls.Add(addButton);

            // Delete Button
            deleteButton = new Button
            {
                Text = "Delete Task",
                Location = new System.Drawing.Point(140, 10),
                Size = new System.Drawing.Size(120, 30)
            };
            deleteButton.Click += DeleteButton_Click;
            this.Controls.Add(deleteButton);

            // View Button
            viewButton = new Button
            {
                Text = "View Task",
                Location = new System.Drawing.Point(270, 10),
                Size = new System.Drawing.Size(120, 30)
            };
            viewButton.Click += ViewButton_Click;
            this.Controls.Add(viewButton);

            // Name TextBox
            nameTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 50),
                Size = new System.Drawing.Size(200, 30)
            };
            this.Controls.Add(nameTextBox);

            // Status TextBox
            statusTextBox = new TextBox
            {
                Location = new System.Drawing.Point(220, 50),
                Size = new System.Drawing.Size(200, 30)
            };
            this.Controls.Add(statusTextBox);

            // Task ListBox
            taskListBox = new ListBox
            {
                Location = new System.Drawing.Point(10, 90),
                Size = new System.Drawing.Size(400, 200)
            };
            this.Controls.Add(taskListBox);

            // Populate task list on form load
            this.Load += (s, e) => task.PopulateTaskList(taskListBox);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string name = nameTextBox.Text.Trim();
            string status = statusTextBox.Text.Trim();
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(status))
            {
                MessageBox.Show("Please enter both task name and status.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            task.AddTask(name, "Description", status, 0.0); // Hardcoded description and due date as in original
            task.PopulateTaskList(taskListBox);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            string name = nameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a task name to delete.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            task.DeleteTask(name);
            task.PopulateTaskList(taskListBox);
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            string name = nameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a task name to view.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            task.PrintTask(name);
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TaskManagerForm());
        }
    }
}