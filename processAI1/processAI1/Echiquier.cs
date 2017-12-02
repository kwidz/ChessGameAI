using System;
using System.Collections.Generic;


/*
 * CLasse d'échiquier
 * Permet de représenter l'échiquier du point de vue du joueur et de trouver les coups possibles 
 * 
 */
public class Echiquier
{
    private const int PP = 10; //pion passant
    private const int P = 1; //pion
    private const int TG = 21; //tour gauche (different pour le roque)
    private const int TD = 22; //tour droite
    private const int CG = 31; //cavalier gauche (différents pour l'image)
    private const int CD = 32; //cavalier droit
    private const int F = 4; //fou
    private const int D = 5; //dame
    private const int R = 6; //roi
   

    public Piece[,] tab;
    List<Piece> mine = new List<Piece>();
    List<String> coups = new List<string>();

	public Echiquier(int[] tabVal)
	{
        tab = new Piece[8,8];
        
        for(int i = 0; i < tabVal.Length; i++)
        {
            int y = i >> 3; //optimisation
            int x = (i & (8 - 1));
            //match all white pawns
            switch(tabVal[i])
            {
                case PP:
                case P:
                    tab[x, y] = new Pion(false,x,y,this);
                    mine.Add(tab[x, y]);
                    break;
                case TG:
                case TD:
                    tab[x, y] = new Tour(true, x, y, this);
                    mine.Add(tab[x, y]);
                    break;

                case CG:
                case CD:
                case F:
                case R:
                case D:
                    tab[x, y] = new Piece(1, false, x, y, this);
                    break;

                case -PP:
                case -P:
                    tab[x, y] = new Pion(true, x, y, this);
                    break;
                case -TG:
                case -TD:
                    tab[x, y] = new Tour(true, x, y, this);
                    break;

                case -CG:
                case -CD:
                case -F:
                case -R:
                case -D:
                    tab[x, y] = new Piece(1, true, x, y, this);

                    break;

                default:
                    tab[x, y] = new Piece(0,true, x, y,this);
                    break;
            }
            
        }
        Console.WriteLine(mine.Count);
	}
    public string playable()
    {
        string allPayableTurns = "";
        foreach(Piece p in mine)
        {

            string tmp = p.myPlays();
            if(tmp!="")
                allPayableTurns += tmp + ";";
        }
        return allPayableTurns;
    }
}
/*
 * 
 * Classe de base pour les pieces basiques 
 * Value (valeur donnée à la piece pour l'évaluation du coup
 * Color : couleur False = blanc
 * Position : Coordonées sur l'échiquier (exemple "a1")
 * x:Coordonnée en x sur l'echiquier (exemple 0 pour "a1")
 * y:Coordonnée en y sur l'echiquier (exemple 0 pour "a1")
 * 
 * La piece possède la référence sur l'échiquier pour pouvoir évaluer la value d'un coup
 * 
 */
public class Piece
{
    protected int value;
    protected bool color;//0=white
    protected string position;
    public int x;
    public int y;
    protected Echiquier e;
    public String[] tabCoord = new string[] { "a8","b8","c8","d8","e8","f8","g8","h8",
                                                   "a7","b7","c7","d7","e7","f7","g7","h7",
                                                   "a6","b6","c6","d6","e6","f6","g6","h6",
                                                   "a5","b5","c5","d5","e5","f5","g5","h5",
                                                   "a4","b4","c4","d4","e4","f4","g4","h4",
                                                   "a3","b3","c3","d3","e3","f3","g3","h3",
                                                   "a2","b2","c2","d2","e2","f2","g2","h2",
                                                   "a1","b1","c1","d1","e1","f1","g1","h1" };
    

    public Piece(int value, bool color, int x, int y,Echiquier e)
    {
        this.value = value;
        this.color = color;
        this.position = tabCoord[y * 8 + x];
        this.x = x;
        this.e = e;
        this.y = y;
    }
    public virtual string myPlays()
    {
        string s = "";
        return s;
    }
    public bool getColor()
    {
        return color;
    }
    public int getValue()
    {
        return this.value;
    }
}

/*
 *
 *Classe pour représenter un pion et ses déplacements  
 * La Classe permetra de pouvoir énnumérer tous les coups possibles d'un pion
 * Pour le moment en passant n'est pas implémenté
 * 
 */
public class Pion : Piece
{
    public Pion(bool color, int x, int y, Echiquier e) : base (10, color,x,y,e)
    {
        
    }
    //Retourne tous les mouvements possible d'un pion 
    public override string myPlays()
    {
        
        if (y > 0 ) {
            Console.WriteLine(x+","+y);
            string s ="";

            if (e.tab[x,y-1].getValue()==0) //pion vers le haut
                s += this.position+ "," + tabCoord[(y - 1 )* 8 + x];
            try
            {
                if (e.tab[x - 1, y - 1].getColor() && e.tab[x - 1, y - 1].getValue() > 0)//prise vers la gauche
                {
                    s += ";" + this.position + ',' + tabCoord[(y - 1 )* 8 + x - 1];
            }
            }
            catch (Exception e) { }
            try
            {
                if (e.tab[x + 1, y - 1].getColor()&& e.tab[x + 1, y - 1].getValue()>0)//prise vers la droite
                {
                    s += ";" + this.position + ',' + tabCoord[(y - 1 )* 8 + x + 1];
                }
            }
            catch (Exception e) { }
            return s;
        }
        return "";
    }
}

public class Tour : Piece
{
    public Tour(bool color, int x, int y, Echiquier e): base(50, color, x, y, e) { }

    public override string myPlays()
    {
        string s = "";
        //déplacement gauche
        int i = 0;
        while ((x - i) > 0)
        {
            i++;
            //prise ou colision
            if (e.tab[x - i, y].getValue() != 0)
            {
                if(e.tab[x - i, y].getColor()){
                    s += this.position + "," + tabCoord[(y) * 8 + x - i] + ';';
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                s += this.position + "," + tabCoord[(y) * 8 + x - i] + ';';
            }
            
        }
        //déplacement droite
        i = 0;
        while ((x + i) < 7)
        {
            i++;
            //prise ou colision
            if (e.tab[x + i, y].getValue() != 0)
            {
                if (e.tab[x + i, y].getColor())
                {
                    s += this.position + "," + tabCoord[(y) * 8 + x + i] + ';';
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                s += this.position + "," + tabCoord[(y) * 8 + x + i]+';';
            }

        }
        //déplacement bas
        i = 0;
        while ((y - i) > 0)
        {
            i++;
            //prise ou colision
            if (e.tab[x , y-i].getValue() != 0)
            {
                if (e.tab[x, y-i].getColor())
                {
                    s += this.position + "," + tabCoord[(y-i) * 8 + x] + ';';
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                s += this.position + "," + tabCoord[(y-i) * 8 + x] + ';';
            }

        }
        //déplacement haut
        i = 0;
        while ((y + i) < 7)
        {
            i++;
            //prise ou colision
            if (e.tab[x, y + i].getValue() != 0)
            {
                if (e.tab[x, y + i].getColor())
                {
                    s += this.position + "," + tabCoord[(y + i) * 8 + x] + ';';
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                s += this.position + "," + tabCoord[(y+i) * 8 + x] + ';';
            }

        }

        return s;
    }

}
