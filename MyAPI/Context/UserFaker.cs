using Bogus;
using MyAPI.Enums;
using MyAPI.Models;

namespace MyAPI.Context
{
    public class UserFaker
    {
        public static List<User> Create(int count)
        {
            //var id = 0;

            Faker<User> fakeUser = new Faker<User>("en_US")
                //.RuleFor(u => u.Id, f => id++)
                .RuleFor(u => u.Name, f => f.Person.FullName)
                .RuleFor(u => u.Login, (f, u) => $"{u.Name!.Split(" ").First()}{u.Id}")
                .RuleFor(u => u.Password, (f, u) => $"{u.Name!.Split(" ").Last()}{u.Id}")
                .RuleFor(u => u.Role, f => f.PickRandom<UserRole>());

            return fakeUser.Generate(count);
        }        
    }
}