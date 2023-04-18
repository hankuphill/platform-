using System;
using System.Linq;
using System.Collections.Generic;



namespace Platform
{
	
	public class Field
	{
		const int ROW = 10;
		const int COL = 10;

		public int[,] Board = new int[ROW, COL];

		public void init()
		{
			for (int row = 0; row < ROW; ++row)
			{
				for (int col = 0; col < COL; ++col)
				{
					Board[row, col] = 0;
				}
			}
		}

		public void display()
		{
			Console.Clear();

			this.drawPlatform();
			this.DrawFallen();


			for (int row = 0; row < ROW; ++row)
			{
				Console.Write("\t\t\t");

				for (int col = 0; col < COL; ++col)
				{
					if (Board[row, col] == 0)
					{
						Console.Write(".");
					}
					else if (Board[row, col] == 9)
					{
						Console.Write(".");
					}
					else if (Board[row,col]==1)
					{
						Console.Write("_");
					}
					else if (Board[row,col]==2)
					{
						Console.Write("*");
					}
					else
					{
						Console.Write(Board[row, col]);
					}
				}

				Console.WriteLine();
			}
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("Wins: {0} Loses: {1}", PlayerWin, PlayerLose);
			Console.WriteLine();
			Console.WriteLine("Right: {0}", Right);
			Console.WriteLine("y: {0} x: {1}", y , x);
			Console.WriteLine("Direction: {0}", Direction);
		}

		public Field()
		{
			this.init();
		}

		public int BasePoint = 3; //for Platform


		public void drawPlatform()
		{

			int PlarformRow = ROW - 1;

			int PlatformPoint1 = BasePoint;
			int PlatformPoint2 = BasePoint + 1;
			int PlatformPoint3 = BasePoint + 2;

			if (BasePoint >= 0 && PlatformPoint3 < COL)
			{
				for (int i = 0; i < COL; ++i)
				{
					Board[PlarformRow, i] = 9;
				}

				//draw platform's points
				Board[PlarformRow, PlatformPoint1] = 1;
				Board[PlarformRow, PlatformPoint2] = 1;
				Board[PlarformRow, PlatformPoint3] = 1;
			}
		}

		public void movePlatform(int LeftRight)
		{
			//check out of bounds
			if (BasePoint >= 0 &
			   (BasePoint + 2) < COL)
			{
				switch (LeftRight)
				{
					case '1':
						if (BasePoint != 0)
							BasePoint -= 1;
						break;

					case '0':
						if ((BasePoint + 3) != COL)
							BasePoint += 1;
						break;

					case '5':
						return;
						break;

				}
			}
		}



		
		public int Direction= -1;
		public bool Right = true;
		
		//cargo coords
		int x = 0;
		int y = 0;
		
		
		int xp,yp=0;
		
		

		public int PlayerWin = 0;
		public int PlayerLose = 0;

		public void DrawFallen()
		{
			if (Direction==-1)
			{
				Random rnd = new Random();
				
				x=0;
				
				Direction =rnd.Next(100);
				if (Direction<50)
				{
					Direction=1;
					
				}
				else
				{
					Direction=2;
				}
				
				int GetRightValue=rnd.Next(10);
				if ( (GetRightValue % 2) == 0)
				{
					Right=!Right;
				}
				
				//get pos for enter of cargo
				bool GoodEnter = false;
				while (!GoodEnter)
				{
					y=rnd.Next(COL);
					if(y==0)
					{
						y+=2;
						GoodEnter=true;
					}
					else if (y==COL)
					{
						--y;
						--y;
						GoodEnter=true;
					}
					else 
					{
						GoodEnter=true;
					}
				}
			}
			//straight falling
			if (Direction==1)
			{
				 // pos for x next x previos
				xp=x-1;
				
				
				if (x==0)
				{
					Board[x,y]=2;
					++x;
				}
				else if (Board[x,y]==1)
				{
					++PlayerWin;
					Board[xp,y]=0;
					Direction=-1;
				}
				else if (Board[x,y]==9)
				{
					++PlayerLose;
					Board[xp,y]=0;
					Direction=-1;
				}
				else
				{
					Board[x,y]=2;
					Board[xp,y]=0;
					++x;
				}
			}
			//when cargo go to the sides
			if(Direction==2)
			{
				xp=x-1;
				
				if(Right)
				{
					yp=y-1;
				}
				else
				{
					yp=y+1;
				}
				
				if (x==0) // first move
				{
					Board[x,y]=2;
					
					++x;
					if (Right)
					{
						++y;
					}
					else
					{
						--y;
					}
				}
				else if (y==COL || y==0)
				{
					Right=!Right;
					
					if(Right)
					{
						y+=2;
						Board[x,y]=2;
						Board[xp,yp]=0;
						++x;
						++y;
					}
					else
					{
						y-=2;
						Board[x,y]=2;
						Board[xp,yp]=0;
						++x;
						--y;
					}
				}
				else if(Board[x,y]!=9 &
						Board[x,y]!=1 &
						y!=0 &
						y!=COL )
				{
					if(Right)
					{
						Board[x,y]=2;
						Board[xp,yp]=0;
						++y;
						++x;
					}
					else
					{
						Board[x,y]=2;
						Board[xp,yp]=0;
						--y;
						++x;
					}
				}
				else if (Board[x,y]==9)
				{
					++PlayerLose;
					Direction=-1;
					Board[xp,yp]=0;
					
				}
				else if (Board[x,y]==1)
				{
					++PlayerWin;
					Direction=-1;
					Board[xp,yp]=0;
				}
				else 
				{
					Console.WriteLine("out of rules!");
					Console.Read();
				}
				
				
			}
		}

	}

}










