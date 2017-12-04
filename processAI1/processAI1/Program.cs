using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace processAI1
{
    class Program
    {

        /* public Coups minimax(int depth, Echiquier e, bool isMyTurn, Coups bestCoup)
         {

             if (depth == 0)
             {
                 return 
             }

         }*/

        static Coups findBestMove(Echiquier e)
        {
            List<Coups> lesCoups = e.playable();



            List<Coups> best = new List<Coups>();
            int BestValue = -900000;
            foreach (Coups c in lesCoups)
            {
                e.playMove(c);
                List<Coups> lesCoupsAdv = e.playableAdversary();
                Coups bestAdv = null;
                int valueAdv = -900000;
                foreach (Coups c2 in lesCoupsAdv)
                {
                    int valuetmp = c2.getValue();
                    if (valuetmp > valueAdv)
                    {
                        valueAdv = valuetmp;
                        bestAdv = c2;
                    }

                }
                int value = c.getValue() - valueAdv;
                if (value == BestValue)
                {
                    best.Add(c);
                }
                if (value > BestValue)
                {
                    BestValue = value;
                    best = new List<Coups>();
                    best.Add(c);
                }
                e.undo();

            }


            Random rnd = new Random();
            int r = rnd.Next(best.Count);
            return best[r];
        }

        static void Main(string[] args)
        {
            try
            {
                bool stop = false;
                int[] tabVal = new int[64];
                String value;
                String[] coord = new String[] { "", "", "" };
                String[] tabCoord = new string[] { "a8","b8","c8","d8","e8","f8","g8","h8",
                                                   "a7","b7","c7","d7","e7","f7","g7","h7",
                                                   "a6","b6","c6","d6","e6","f6","g6","h6",
                                                   "a5","b5","c5","d5","e5","f5","g5","h5",
                                                   "a4","b4","c4","d4","e4","f4","g4","h4",
                                                   "a3","b3","c3","d3","e3","f3","g3","h3",
                                                   "a2","b2","c2","d2","e2","f2","g2","h2",
                                                   "a1","b1","c1","d1","e1","f1","g1","h1" };

                while (!stop)
                {
                    using (var mmf = MemoryMappedFile.OpenExisting("plateau"))
                    {
                        using(var mmf2 = MemoryMappedFile.OpenExisting("repAI1"))
                        {
                            Mutex mutexStartAI1 = Mutex.OpenExisting("mutexStartAI1");
                            Mutex mutexAI1 = Mutex.OpenExisting("mutexAI1");
                            mutexAI1.WaitOne();
                            
                            mutexStartAI1.WaitOne();

                            using (var accessor = mmf.CreateViewAccessor())
                            {
                                ushort Size = accessor.ReadUInt16(0);
                                byte[] Buffer = new byte[Size];
                                accessor.ReadArray(0 + 2, Buffer, 0, Buffer.Length);

                                value = ASCIIEncoding.ASCII.GetString(Buffer);
                                if (value == "stop") stop = true;
                                else
                                {
                                    String[] substrings = value.Split(',');
                                    for (int i = 0; i < substrings.Length; i++)
                                    {
                                        tabVal[i] = Convert.ToInt32(substrings[i]);
                                    }
                                }
                            }
                            if (!stop)
                            {
                                /******************************************************************************************************/
                                /***************************************** ECRIRE LE CODE DE L'IA *************************************/
                                /******************************************************************************************************/


                                Coups best;
                                Agent a = new Agent();
                                a.Percept(tabVal);
                                a.SearchBestMove();
                                best = a.joue();
                                    


                                
                                coord[0] = best.positionDepart;
                                coord[1] = best.positionArrivee;
                                coord[2] = "D";
                                
                                

                                
                                /********************************************************************************************************/
                                /********************************************************************************************************/
                                /********************************************************************************************************/

                                using (var accessor = mmf2.CreateViewAccessor())
                                {
                                    value = coord[0];
                                    for (int i = 1; i < coord.Length; i++)
                                    {
                                        value += "," + coord[i];
                                        
                                    }
                                    
                                    byte[] Buffer = ASCIIEncoding.ASCII.GetBytes(value);
                                    
                                    accessor.Write(0, (ushort)Buffer.Length);
                                    accessor.WriteArray(0 + 2, Buffer, 0, Buffer.Length);
                                }
                            }
                            mutexAI1.ReleaseMutex();
                            mutexStartAI1.ReleaseMutex();
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Memory-mapped file does not exist. Run Process A first.");
                Console.ReadLine();
            }
        }
    }
}
