using Bogus;
using RestSharp;
using SwapMe.Application.Handlers.Users.Requests;

namespace SwapMe.Unit.Population;

public class UsersPopulation
{
    private readonly RestClient _usersRestClient;
    private const string UsersUrl = "http://localhost:5147/api/users";
    
    public UsersPopulation()
    {
        _usersRestClient = new RestClient(UsersUrl);
    }
    
    [Fact]
    public async Task PopulateUsers()
    {
        const int GeneratedUsers = 1000;
        
        var usersFaker = new Faker<CreateUserRequest>("pl")
            .CustomInstantiator(f => new CreateUserRequest(
                f.Internet.UserName(),
                f.Internet.Password(),
                f.Person.FirstName,
                f.Person.LastName,
                f.Internet.Email(f.Person.FirstName, f.Person.LastName),
                f.Phone.PhoneNumber("#########"),
                f.Address.City(),
                f.Address.State()
            ));

        var users = usersFaker.Generate(GeneratedUsers);

        foreach (var request in users.Select(user => new RestRequest("", Method.Post)
                     .AddJsonBody(user)))
        {
            await _usersRestClient.PostAsync(request);
        }
    }
}