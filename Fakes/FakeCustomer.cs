
using Bogus;
namespace ApiLiteDB.Fakes
{
    public static class FakeCustomer
    {
        public static List<Customer> ListarClientesFake(int quantidade)
        {
            var clienteFaker = new Faker<Customer>("pt_BR")
               .RuleFor(c => c.Name, f => f.Name.FullName())
               .RuleFor(c => c.BirthDate, f => f.Date.Recent(100).ToShortDateString())
               .RuleFor(c => c.Gender, f => f.Person.Gender.ToString())
               .RuleFor(c => c.Address, f => $"{f.Person.Address.Street}, {f.Address.BuildingNumber()} - CEP {f.Person.Address.ZipCode}")
               .RuleFor(c => c.City, f => f.Person.Address.City)
               .RuleFor(c => c.State, f => f.Person.Address.State)
               .RuleFor(c => c.Country, f => "Brasil")
               .RuleFor(c => c.Email, (f, c) =>  f.Internet.Email(c.Name).ToLower())
               .RuleFor(c => c.Avatar, f => f.Internet.Avatar())
               .RuleFor(c => c.UserName, (f, c) => f.Internet.UserName(c.Name))
               .RuleFor(c => c.Phones, f => f.Make(3, ()=> f.Phone.PhoneNumber()).ToArray())
               .RuleFor(c => c.IsActive, f => true);

            var clientes = clienteFaker.Generate(quantidade);

            return clientes;
        }
    }
}