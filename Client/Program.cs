
namespace wm
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Client client = new Client("Warren");
            client.Connect("127.0.0.1", 5000);
            client.StartClient();
        }
    }
}