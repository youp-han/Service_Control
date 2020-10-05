using System;
using ScmManagerServiceControl.Controller;



namespace ScmManagerServiceControl
{
    class Program
    {
        static void Main(string[] args)
        {
            StartProcessController start = new StartProcessController();

            try
            {
                start.StartProcess();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
