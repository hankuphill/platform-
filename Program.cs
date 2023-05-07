using System;
using System.Linq;

namespace Platform
{

	public static class Program
	{
		public static void Main()
		{
			const int Q_ROUND = 5;
			bool Win = false;
			bool Lose = false;

			Field field = new Field();

			Memoria mem = new Memoria();

			Neuron[] LayerOne = new Neuron[10];
			Neuron[] LayerTwo = new Neuron[2];

			int SizeOfIn = field.Board.Length;

			int CircileCount = 0;

			int AI_Move = 0;//Console.Read();

			int CurWin = field.PlayerWin;
			int CurLose = field.PlayerLose;

			double Error = 0.004;
			double ErrorPrev = 0.0;

			double LR = 0.01; //Learning Rate

			double Epoch = 1.0;
			double Iteration = 0.0;

			double[] In = new double[SizeOfIn];

			for (int i = 0; i < LayerOne.Length; ++i)
			{
				LayerOne[i] = new Neuron(field.Board.Length);
			}

			for (int iTwo = 0; iTwo < LayerTwo.Length; ++iTwo)
			{
				LayerTwo[iTwo] = new Neuron(LayerOne.Length);
			}

			bool LearnigState = false;
			string LSStatus = "";
			string LSStatus2 = "";
			double[] MemPeekRes = new double[SizeOfIn];

			bool GameOn = true;
			while (GameOn)
			{
				if (LearnigState == false)
				{
					field.display();

					++Iteration;
					if (Iteration == Field.ROW)
					{
						++Epoch;
						Iteration = 0;
					}

					field.InfoLine2 = "Epoch: " + Epoch + " Iteration: " + Iteration + "  Learning status: " + LearnigState;


					int IndexIn = 0;
					for (int r = 0; r < Field.ROW; ++r)
					{
						for (int c = 0; c < Field.COL; ++c)
						{
							if (field.Board[r, c] == 9) //lost zone
							{
								In[IndexIn] = -0.02;
							}
							else if (field.Board[r, c] == 0) //empy
							{
								In[IndexIn] = 0.0;
							}
							else if (field.Board[r, c] == 1)//platform
							{
								In[IndexIn] = 0.01;
							}
							else if (field.Board[r, c] == 2)//  *
							{
								In[IndexIn] = 0.02;
							}
							++IndexIn;
						}
					}
					if (LearnigState == false)
					{
						mem.push(In);
					}

				} // Learning State = false

				if (LearnigState == true)
				{
					MemPeekRes = mem.peek();
					In = mem.pop();
				}

				for (int i = 0; i < LayerOne.Length; ++i)
				{
					LayerOne[i].input(In);
					if (LearnigState == false)
					{
						Console.WriteLine("L1-N[{0}] {1} sum: {2}", i, LayerOne[i].Output, LayerOne[i].OutSum);
					}
				}



				for (int iTwo = 0; iTwo < LayerTwo.Length; ++iTwo)
				{
					LayerTwo[iTwo].input(LayerOne);

					string LorR;
					if (iTwo == 0)
					{
						LorR = "LEFT";
					}
					else
					{
						LorR = "RIGHT";
					}
					if (LearnigState == false)
					{
						Console.WriteLine("L2-N[{0}] out: {1} {2}", iTwo, LayerTwo[iTwo].Output, LorR);
					}
				}


				//-----------move part
				if (LayerTwo[0].Output > LayerTwo[1].Output)
				{
					AI_Move = 1;
					field.movePlatform(AI_Move);

					field.InfoLine = "1-LEFT";


					if (LearnigState == true)
					{
						double CurrentError = GetError();
						if (Win == true)
						{
							LayerTwo[0].correctWeights(LayerOne, CurrentError);
						}
						LayerTwo[1].correctWeights(LayerOne, CurrentError);

						CorrectHidden(LayerOne);
					}
				}
				else if (LayerTwo[0].Output < LayerTwo[1].Output)
				{
					AI_Move = 0;
					field.movePlatform(AI_Move);

					field.InfoLine = "0-RIGHT";



					if (LearnigState == true)
					{
						double CurrentError = GetError();
						if (Win == true)
						{
							LayerTwo[0].correctWeights(LayerOne, CurrentError);
						}
						LayerTwo[1].correctWeights(LayerOne, CurrentError);

						CorrectHidden(LayerOne);
					}
				}
				else
				{
					AI_Move = 6; //stay on one place
					field.InfoLine = "MIDDLE";

				}

				if (field.InfoLine.Length > 50)
				{
					field.InfoLine = "";
				}


				//	field.movePlatform(AI_Move);


				if (LearnigState == false)
				{
					CircileCount++;
					if (CircileCount > Q_ROUND)
					{
						int goEXIT = Console.Read();

						if (goEXIT == 1)
						{
							return;
						}
						CircileCount = 0;
					}
				}
				else
				{
					Console.Clear();
					LSStatus = "Learning Status: " + LearnigState.ToString();
					LSStatus2 += mem.Counter + " ";

					Console.WriteLine(LSStatus);
					Console.Write(LSStatus2);
					Console.WriteLine();

					//foreach (var value in MemPeekRes)
					//{
					//Console.Write(value);
					//Console.Write("-");
					//}

					//Console.Read();
				}

				//game status changed
				if (CurWin < field.PlayerWin)
				{
					Win = true;

					LearnigState = true;
					++CurWin;

					Error = -0.04 * LR * Epoch;
				}
				else if (CurLose < field.PlayerLose)
				{
					Lose = true;

					LearnigState = true;
					++CurLose;

					Error = 0.04 * LR * Epoch;
				}

				//learning ended
				if (mem.Counter == 0)
				{
					LearnigState = false;

					Win = false;
					Lose = false;
				}

			}// end while




			void CorrectHidden(Neuron[] InLayer)
			{
				for (int i = 0; i < InLayer.Length; ++i)
				{
					InLayer[i].correctWeights(LayerTwo, -1, 1);
				}
			}


			double GetError()
			{
				//Error=Error*LR*Epoch;

				Error = Math.Pow((Error - 1), 2.0) + 1;

				return sigmoid(Error);
			}

			// old version
			double GetError2()
			{
				double diferencia = Math.Abs(Error - ErrorPrev);

				if (diferencia != LR)
				{
					if (Error < ErrorPrev) //gradient go down
					{
						ErrorPrev = Error;
						Error += LR;
					}
					else if (Error > ErrorPrev) // go up
					{
						ErrorPrev = Error;
						Error -= LR;
					}
				}
				// y=(x-1)²+1;:
				Error = Math.Pow((Error - 1), 2.0) + 1;

				return sigmoid(Error);

			}




			float sigmoid(double value)
			{
				return (float)(1.0 / (1.0 + Math.Pow(Math.E, -value)));
			}

		}
	}
}









