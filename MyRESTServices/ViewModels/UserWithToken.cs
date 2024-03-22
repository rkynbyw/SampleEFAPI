namespace MyRESTServices.ViewModels
{
    public class UserWithToken
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public List<string>? Roles { get; set; } // Menambahkan properti untuk menyimpan daftar roles

        public string? Token { get; set; }
    }

}
