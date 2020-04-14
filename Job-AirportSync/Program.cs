using Jobs_Common_Libs;

namespace Job_Task_One
{
    class Program : Job
    {
        static void Main(string[] args)
        {
            WriteLine($"Child Process - {ProcessName} {string.Join(" ", args)}");
        }
    }
}
