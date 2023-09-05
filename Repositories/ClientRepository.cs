using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public interface IClientRepository
    {
        Task<List<Client>> Get(string keyWord);
        Task<int> Create(List<Client> lstClient);
        Task<string> Update(Client client);
        Task SendEmail(string email);
    }

    public class ClientRepository : IClientRepository
    {
        private readonly DataContext dataContext;
        private readonly IEmailRepository emailRepository;
        private readonly IDocumentRepository documentRepository;

        public ClientRepository(DataContext dataContext, IEmailRepository emailRepository, IDocumentRepository documentRepository)
        {
            this.dataContext = dataContext;
            this.emailRepository = emailRepository;
            this.documentRepository = documentRepository;
        }

        public async Task<int> Create(List<Client>lstClient)
        {
            int result = 0;
            //check if all properties of 
            //client model class has value
            //since all fields are required
            foreach(var client in lstClient)
            { 
                bool isClientModelNotEmpty = client.GetType().GetProperties().All(x => x.GetValue(client) != null);
                //if yes then proceed with creating user
                if (isClientModelNotEmpty)
                {
                    await dataContext.Clients.AddAsync(client);
                    await dataContext.SaveChangesAsync();
                    //send email after client is created
                    //await SendEmail(client.Email);
                }
                else
                {
                    result = -1;
                }
            }

            return result;
        }

        public Task<List<Client>> Get(string keyWord)
        {
            //return all records that matches the keyword
            //with either first name or last name
            var results = dataContext.Clients.Where(x => x.FirstName.ToLower().Contains(keyWord.ToLower()) 
                || x.LastName.ToLower().Contains(keyWord.ToLower())).ToList();
            return Task.Run(() => results);
        }

        public async Task<string> Update(Client client)
        {
            string newEmailAddress = string.Empty;
            var existingClient = await dataContext.Clients.FirstOrDefaultAsync(x => x.Id == client.Id);

            if (existingClient != null)
            {
                existingClient.FirstName = client.FirstName;
                existingClient.LastName = client.LastName;
                existingClient.Email = "j.a.zec27@gmail.com";
                existingClient.PhoneNumber = "09081916278";
                //get value of new email address
                newEmailAddress = existingClient.Email;

                await dataContext.SaveChangesAsync();
            }
            return newEmailAddress;
        }
        public async Task SendEmail(string emailAddress)
        {
            await emailRepository.Send(emailAddress, "Hi there - welcome to my Carepatron portal.");
            await documentRepository.SyncDocumentsFromExternalSource(emailAddress);
        }
    }
}

