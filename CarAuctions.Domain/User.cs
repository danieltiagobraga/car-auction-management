namespace CarAuctions.Domain;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; }
    public string UserName { get; set; }

    protected User(int id, string name, string email, string password, string userName) 
    {
        Id = id;
        Name = name;
        Email = email;
        Password = password;
        UserName = userName;
    }
}
