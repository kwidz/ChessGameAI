using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Agent
{
    Echiquier environement;
    Coups Ajouer;

    public Agent()
    {

    }
    public void Percept(int[] tabVal)
    {
        environement = new Echiquier(tabVal);
    }

    public Coups joue()
    {
        return Ajouer;
    }


    public void SearchBestMove()
    {
        Ajouer = minimaxFirst(2, environement, true);
    }

    private int minimax(int depth, Echiquier e, int alpha, int beta, Boolean player)
    {

        if (depth == 0)
        {
            return e.evaluate();
        }
        if (player)
        {
            int bestValue = -9000;
            List<Coups> lesCoups = e.playable();
            foreach (Coups c in lesCoups)
            {
                e.playMove(c);
                bestValue = Math.Max(bestValue, minimax(depth - 1, e, alpha, beta, !player));
                e.undo();
                alpha = Math.Max(alpha, bestValue);
                if (beta <= alpha)
                {
                    return bestValue;
                }
            }
            return bestValue;
        }
        else
        {
            int bestValue = 9000;
            List<Coups> lesCoups = e.playableAdversary();
            foreach (Coups c in lesCoups)
            {
                e.playMove(c);
                bestValue = Math.Min(bestValue, minimax(depth - 1, e, alpha, beta, !player));
                e.undo();
                beta = Math.Min(beta, bestValue);
                if (beta <= alpha)
                {
                    return bestValue;
                }
            }
            return bestValue;
        }

    }


    private Coups minimaxFirst(int depth, Echiquier e, Boolean player)
    {
        int bestValue = -9000;
        Coups leBest = null;
        List<Coups> lesCoups = e.playable();
        foreach (Coups c in lesCoups)
        {
            e.playMove(c);
            int value = minimax(depth - 1, e, -10000, 10000, !player);
            e.undo();
            if (value > bestValue)
            {
                leBest = c;
                bestValue = value;
            }
            if (value == bestValue)
            {
                Random rand = new Random();
                if (rand.Next(0, 2) == 0)
                {
                    leBest = c;
                    bestValue = value;
                }

            }
        }
        return leBest;
    }


}

