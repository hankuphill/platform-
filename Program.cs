using System;
using System.Linq;

namespace Platform
{
	
	public static class Program
	{
		public static void Main()
		{
			Field field = new Field();
			
			//GameOn Circle!
			
			bool GameOn = true;
			
			while(GameOn)
			{
				field.display();
				
				Console.WriteLine();
				Console.WriteLine("Enter 1 for left or 2 for right");
				
				//int UserInput=0;
				
				int UserInput=Console.Read();
				
				
				field.movePlatform(UserInput);
			}

		}
	}
}
