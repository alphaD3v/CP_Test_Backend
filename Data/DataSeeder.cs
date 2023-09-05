using api.Models;

namespace api.Data
{
    public class DataSeeder
    {
        private readonly DataContext dataContext;

        public DataSeeder(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void Seed(List<Client> lstClient)
        {
            foreach (var client in lstClient)
            {
                //var client = new Client("xosiosiosdhad", "John", "Smith", "john@gmail.com", "+18202820232");
                bool isClientModelNotEmpty = client.GetType().GetProperties().All(x => x.GetValue(client) != null);
                if (isClientModelNotEmpty)
                {
                    dataContext.Add(client);
                    dataContext.SaveChanges();
                }
            }
        }
    }
}

