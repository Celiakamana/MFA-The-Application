using System;
using Microsoft.Maui.Controls;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;



namespace MFA_The_Application
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        // This method will be executed when the 'Register' button is clicked
        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrationPage());
        }

        // This method will be executed when the 'Login' button is clicked
        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            // Getting input values from the Entry fields
            string phoneNumber = PhoneNumberLoginEntry.Text;
            string password = PasswordLoginEntry.Text;

            // Validating that both fields are filled
            if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Please enter both phone number and password", "OK");
                return;
            }

            // Hash the entered password to able to match it to the hashed password stored in DB
            string hashedPassword = HashPassword(password);

            //Try/catch box to handle any expections or Login failure
            try
            {
                // Connection string to the database
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=UserRegistrationDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

                // Query to check if the phone number and hashed password match an existing record
                string query = @"SELECT COUNT(1) FROM UsersTable WHERE PhoneNumber = @PhoneNumber AND PasswordHash = @PasswordHash";

                //opening a connection
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    //commads to execute the query and perform login
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        //to prevent SQL injection
                        cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                        //Execute the query 
                        int count = Convert.ToInt32 (await cmd.ExecuteScalarAsync());
                        
                        // If a match is found, redirect to TransactionPage
                        if (count == 1)
                        {
                            await Navigation.PushAsync(new TransactionsPage());  
                        }
                        else
                        {
                            // If no match is found, show an error message
                            await DisplayAlert("Error", "Invalid phone number or password", "OK");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any other errors that occur during login i.e database interaction
                await DisplayAlert("Error", $"Database error: {ex.Message}", "OK");
            }
        }

        // Method to Hash the password using SHA256 (same hashing method used during registration)
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }


}
