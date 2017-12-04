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
    public String[] tabCoord = new string[] { "a8","b8","c8","d8","e8","f8","g8","h8",
                                                   "a7","b7","c7","d7","e7","f7","g7","h7",
                                                   "a6","b6","c6","d6","e6","f6","g6","h6",
                                                   "a5","b5","c5","d5","e5","f5","g5","h5",
                                                   "a4","b4","c4","d4","e4","f4","g4","h4",
                                                   "a3","b3","c3","d3","e3","f3","g3","h3",
                                                   "a2","b2","c2","d2","e2","f2","g2","h2",
                                                   "a1","b1","c1","d1","e1","f1","g1","h1" };


    public Piece[,] tab;
    List<Piece> mine = new List<Piece>();
    public List<Piece> yours = new List<Piece>();
    List<Echiquier> olds = new List<Echiquier>();

    public List <Coups> played= new List<Coups>();

    public List<Coups> LesCoups = new List<Coups>();

    public Echiquier() { }

    public int evaluate()
    {
        int evaluation = 0;
        for(int i = 0; i<8; i++)
        {
            for(int j=0; j<8; j++)
            {
                if (!tab[i, j].getColor())
                {
                    evaluation += tab[i, j].getValue();
                }
                else
                {
                    evaluation -= tab[i, j].getValue();
                }
            }
        }
        Console.WriteLine(evaluation);
        return evaluation;
    }

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
                    tab[x, y] = new Tour(false, x, y, this);
                    mine.Add(tab[x, y]);
                    break;

                case CG:
                case CD:
                    tab[x, y] = new Cavalier(false, x, y, this);
                    mine.Add(tab[x, y]);
                    break;
                case F:
                    tab[x, y] = new Fou(false, x, y, this);
                    mine.Add(tab[x, y]);
                    break;
                case D:
                    tab[x, y] = new Dame(false, x, y, this);
                    mine.Add(tab[x, y]);
                    break;

                case R:

                    tab[x, y] = new Roi(false, x, y, this);
                    mine.Add(tab[x, y]);
                    break;

                case -PP:
                case -P:
                    tab[x, y] = new PionNoir(true, x, y, this);
                    yours.Add(tab[x, y]);
                    break;
                case -TG:
                case -TD:
                    tab[x, y] = new Tour(true, x, y, this);
                    yours.Add(tab[x, y]);
                    break;

                case -CG:
                case -CD:
                    tab[x, y] = new Cavalier(true, x, y, this);
                    yours.Add(tab[x, y]);
                    break;
                case -F:
                    tab[x, y] = new Fou(true, x, y, this);
                    yours.Add(tab[x, y]);
                    break;
                case -D:
                    tab[x, y] = new Dame(true, x, y, this);
                    yours.Add(tab[x, y]);
                    break;
                case -R:

                    tab[x, y] = new Roi(true, x, y, this);
                    yours.Add(tab[x, y]);
                    break;

                default:
                    tab[x, y] = new Piece(0,true, x, y,this);
                    break;
            }
            
        }
       
	}
    public List<Coups> playable()
    {
        foreach(Piece p in mine)
        {

            p.myPlays();
        }
        List < Coups > l = new List<Coups>(LesCoups);
        LesCoups = new List<Coups>();
        return l;
    }

    public List<Coups> playableAdversary()
    {
        foreach (Piece p in yours)
        {
            p.yourPlays();
        }
        List<Coups> l = new List<Coups>(LesCoups);
        LesCoups = new List<Coups>();
        return l;
    }

    public void playMove(Coups c)
    {
        played.Add(c);
        if (c.getValue() > 0)
        {
            //les blancs jouent
            if (c.prise.getColor())
            {

                yours.Remove(c.prise);
                Piece p = tab[c.xDepart, c.yDepart];
                p.x = c.xArrivee;
                p.y = c.yArrivee;
                tab[c.xArrivee, c.yArrivee] = p;
                tab[c.xDepart, c.yDepart] = new Piece(0, true, c.xDepart, c.yDepart, this);
            }
            //les noirs jouent
            else
            {
                mine.Remove(c.prise);
                Piece p = tab[c.xDepart, c.yDepart];
                p.x = c.xArrivee;
                p.y = c.yArrivee;
                tab[c.xArrivee, c.yArrivee] = p;
                tab[c.xDepart, c.yDepart] = new Piece(0, true, c.xDepart, c.yDepart, this);
            }
        }
        else
        {
            Piece p = tab[c.xDepart, c.yDepart];
            tab[c.xDepart, c.yDepart] = tab[c.xArrivee, c.yArrivee];
            tab[c.xArrivee, c.yArrivee] = p;
            p.x = c.xArrivee;
            p.y = c.yArrivee;
            tab[c.xDepart, c.yDepart].x = c.xDepart;
            tab[c.xDepart, c.yDepart].y = c.xDepart;
        }
        
        
    }
    public void undo()
    {
        Coups c = played[played.Count-1];
        played.Remove(c);
        if (c.getValue() > 0)
        {
            //les blancs jouent
            if (c.prise.getColor())
            {

                yours.Add(c.prise);
                Piece p = tab[c.xArrivee, c.yArrivee];
                p.x = c.xDepart;
                p.y = c.yDepart;
                tab[c.xDepart, c.yDepart] = p;
                tab[c.xArrivee, c.yArrivee] = c.prise;
                c.prise.x = c.xArrivee;
                c.prise.y = c.yArrivee;
            }
            //les noirs jouent
            else
            {
                mine.Add(c.prise);
                Piece p = tab[c.xArrivee, c.yArrivee];
                p.x = c.xDepart;
                p.y = c.yDepart;
                tab[c.xDepart, c.yDepart] = p;
                tab[c.xArrivee, c.yArrivee] = c.prise;
                c.prise.x = c.xArrivee;
                c.prise.y = c.yArrivee;
            }
        }
        else
        {
            Piece p = tab[c.xDepart, c.yDepart];
            tab[c.xDepart, c.yDepart] = tab[c.xArrivee, c.yArrivee];
            tab[c.xArrivee, c.yArrivee] = p;
            p.x = c.xArrivee;
            p.y = c.yArrivee;
            tab[c.xDepart, c.yDepart].x = c.xDepart;
            tab[c.xDepart, c.yDepart].y = c.xDepart;
        }


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
    public virtual void myPlays()
    {
        
    }
    public virtual void yourPlays()
    {

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
    public override void myPlays()
    {
        
        if (y > 0 ) {
            


            if (e.tab[x, y - 1].getValue() == 0) //pion vers le haut
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y - 1) * 8 + x], this.x, this.y, x, y-1,(e.tab[x, y - 1])));
                
            try
            {
                if (e.tab[x - 1, y - 1].getColor() && e.tab[x - 1, y - 1].getValue() > 0)//prise vers la gauche
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y - 1) * 8 + x-1], this.x, this.y, x-1, y - 1, (e.tab[x-1, y - 1])));
                    
            }
            }
            catch (Exception e) { }
            try
            {
                if (e.tab[x + 1, y - 1].getColor()&& e.tab[x + 1, y - 1].getValue()>0)//prise vers la droite
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y - 1) * 8 + x+1], this.x, this.y, x+1, y - 1, (e.tab[x+1, y - 1])));
                }
            }
            catch (Exception e) { }
            
        }
       
    }
    public override void yourPlays()
    {

        if (y > 0)
        {
            



            if (e.tab[x, y - 1].getValue() == 0) //pion vers le haut
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y - 1) * 8 + x], this.x, this.y, x, y - 1, (e.tab[x, y - 1])));

            try
            {
                if (!(e.tab[x - 1, y - 1].getColor()) && e.tab[x - 1, y - 1].getValue() > 0)//prise vers la gauche
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y - 1) * 8 + x - 1], this.x, this.y, x - 1, y - 1, (e.tab[x - 1, y - 1])));

                }
            }
            catch (Exception e) { }
            try
            {
                if (!(e.tab[x + 1, y - 1].getColor()) && e.tab[x + 1, y - 1].getValue() > 0)//prise vers la droite
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y - 1) * 8 + x + 1], this.x, this.y, x + 1, y - 1, (e.tab[x + 1, y - 1])));
                }
            }
            catch (Exception e) { }

        }

    }
}
public class PionNoir : Piece
{
    public PionNoir(bool color, int x, int y, Echiquier e) : base(10, color, x, y, e)
    {

    }
    //Retourne tous les mouvements possible d'un pion 
    public override void myPlays()
    {

        if (y < 7)
        {
            
            string s = "";

            if (e.tab[x, y + 1].getValue() == 0) //pion vers le haut
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y + 1) * 8 + x], this.x, this.y, x, y+ 1, (e.tab[x, y + 1])));
            try
            {
                if (e.tab[x - 1, y + 1].getColor() && e.tab[x - 1, y + 1].getValue() > 0)//prise vers la gauche
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y + 1) * 8 + x-1], this.x, this.y, x-1, y + 1, (e.tab[x-1, y + 1])));
                }
            }
            catch (Exception e) { }
            try
            {
                if (e.tab[x + 1, y + 1].getColor() && e.tab[x + 1, y + 1].getValue() > 0)//prise vers la droite
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y+ 1) * 8 + x+1], this.x, this.y, x+1, y + 1, (e.tab[x+1, y +1])));
                }
            }
            catch (Exception e) { }
            
        }
        
    }
 
    public override void yourPlays()
    {

        if (y < 7)
        {
            
            string s = "";

            if (e.tab[x, y + 1].getValue() == 0) //pion vers le haut
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y + 1) * 8 + x], this.x, this.y, x, y + 1, (e.tab[x, y + 1])));
            try
            {
                if (!(e.tab[x - 1, y + 1].getColor()) && e.tab[x - 1, y + 1].getValue() > 0)//prise vers la gauche
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y + 1) * 8 + x - 1], this.x, this.y, x - 1, y + 1, (e.tab[x - 1, y + 1])));
                }
            }
            catch (Exception e) { }
            try
            {
                if (!(e.tab[x + 1, y + 1].getColor()) && e.tab[x + 1, y + 1].getValue() > 0)//prise vers la droite
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y + 1) * 8 + x + 1], this.x, this.y, x + 1, y + 1, (e.tab[x + 1, y + 1])));
                }
            }
            catch (Exception e) { }

        }

    }
}

public class Tour : Piece
{
    public Tour(bool color, int x, int y, Echiquier e): base(50, color, x, y, e) { }

    public override void myPlays()
    {
        
        //déplacement gauche
        int i = 1;
        while ((x - i) >= 0)
        {
            
            //prise ou colision

            if (e.tab[x - i, y].getValue() != 0)
            {
                if(e.tab[x - i, y].getColor()){
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y) * 8 + x-i], this.x, this.y, x-i, y , (e.tab[x-i, y])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y) * 8 + x-i], this.x, this.y, x-i, y, (e.tab[x-i, y])));
            }

            i++;

        }
        //déplacement droite
        i = 1;
        while ((x + i) < 8)
        {
            
            //prise ou colision
            if (e.tab[x + i, y].getValue() != 0)
            {
                if (e.tab[x + i, y].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y) * 8 + x+i], this.x, this.y, x+i, y, (e.tab[x+i, y])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y) * 8 + x + i], this.x, this.y, x+i, y, (e.tab[x + i, y])));
            }
            i++;

        }
        //déplacement bas
        i = 1;
        while ((y - i) >= 0)
        {
            
            //prise ou colision
            if (e.tab[x , y-i].getValue() != 0)
            {
                if (e.tab[x, y-i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y-i) * 8 + x], this.x, this.y, x, y - i, (e.tab[x, y-i])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y - i) * 8 + x], this.x, this.y, x, y - i, (e.tab[x, y - i])));

            }
            i++;

        }
        //déplacement haut
        i = 1;
        while ((y + i) < 8)
        {
            //prise ou colision
            if (e.tab[x, y + i].getValue() != 0)
            {
                if (e.tab[x, y + i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x], this.x, this.y, x, y +i, (e.tab[x, y + i])));

                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x], this.x, this.y, x, y +i, (e.tab[x, y + i])));
            }
            i++;

        }

      
    }
    public override void yourPlays()
    {

        //déplacement gauche
        int i = 1;
        while ((x - i) >= 0)
        {

            //prise ou colision

            if (e.tab[x - i, y].getValue() != 0)
            {
                if (!e.tab[x - i, y].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y) * 8 + x - i], this.x, this.y, x - i, y, (e.tab[x - i, y])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y) * 8 + x - i], this.x, this.y, x - i, y, (e.tab[x - i, y])));
            }

            i++;

        }
        //déplacement droite
        i = 1;
        while ((x + i) < 8)
        {

            //prise ou colision
            if (e.tab[x + i, y].getValue() != 0)
            {
                if (!e.tab[x + i, y].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y) * 8 + x + i], this.x, this.y, x + i, y, (e.tab[x + i, y])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y) * 8 + x + i], this.x, this.y, x + i, y, (e.tab[x + i, y])));
            }
            i++;

        }
        //déplacement bas
        i = 1;
        while ((y - i) >= 0)
        {

            //prise ou colision
            if (e.tab[x, y - i].getValue() != 0)
            {
                if (!e.tab[x, y - i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y - i) * 8 + x], this.x, this.y, x, y - i, (e.tab[x, y - i])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y - i) * 8 + x], this.x, this.y, x, y - i, (e.tab[x, y - i])));

            }
            i++;

        }
        //déplacement haut
        i = 1;
        while ((y + i) < 8)
        {
            //prise ou colision
            if (e.tab[x, y + i].getValue() != 0)
            {
                if (!e.tab[x, y + i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x], this.x, this.y, x, y + i, (e.tab[x, y + i])));

                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x], this.x, this.y, x, y + i, (e.tab[x, y + i])));
            }
            i++;

        }


    }

}
public class Cavalier : Piece
{
    public Cavalier(bool color, int x, int y, Echiquier e) : base(30, color, x, y, e) { }
    public override void myPlays()
    {
        string s = "";

        int[][] offsets = new int[][] {
        new int[] {-2, 1},
        new int[] {-1, 2},
        new int[] {1, 2},
        new int[] {2, 1},
        new int[] {2, -1},
        new int[] {1, -2},
        new int[] {-1, -2},
        new int[] {-2, -1}
    };
        foreach (int[] o in offsets)
        {

            int moveX = o[0];
            int moveY = o[1];
            if (0 <= x + moveX && x + moveX <= 7 && y + moveY >= 0 && y + moveY <= 7)
                //Si prise ou colision
                if (e.tab[x + moveX, y + moveY].getValue() != 0)
                {
                    if (e.tab[x + moveX, y + moveY].getColor())
                    {
                        e.LesCoups.Add(new Coups(this.position, tabCoord[(y + moveY) * 8 + x+moveX], this.x, this.y, x+moveX, y+moveY, (e.tab[x+moveX, y +moveY])));

                    }
                }
                else
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y + moveY) * 8 + x + moveX], this.x, this.y, x+moveX, y+moveY, (e.tab[x + moveX, y + moveY])));
                    

                }
        }
        
    }
    public override void yourPlays()
    {
        string s = "";

        int[][] offsets = new int[][] {
        new int[] {-2, 1},
        new int[] {-1, 2},
        new int[] {1, 2},
        new int[] {2, 1},
        new int[] {2, -1},
        new int[] {1, -2},
        new int[] {-1, -2},
        new int[] {-2, -1}
    };
        foreach (int[] o in offsets)
        {

            int moveX = o[0];
            int moveY = o[1];
            if (0 <= x + moveX && x + moveX <= 7 && y + moveY >= 0 && y + moveY <= 7)
                //Si prise ou colision
                if (e.tab[x + moveX, y + moveY].getValue() != 0)
                {
                    if (!e.tab[x + moveX, y + moveY].getColor())
                    {
                        e.LesCoups.Add(new Coups(this.position, tabCoord[(y + moveY) * 8 + x + moveX], this.x, this.y, x + moveX, y + moveY, (e.tab[x + moveX, y + moveY])));

                    }
                }
                else
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y + moveY) * 8 + x + moveX], this.x, this.y, x + moveX, y + moveY, (e.tab[x + moveX, y + moveY])));


                }
        }

    }
}
public class Fou : Piece
{
    public Fou(bool color, int x, int y, Echiquier e) : base(30, color, x, y, e) { }
    public override void myPlays()
    {
        
        //déplacement gauche/haut
        int i = 1;
        while ((x - i) >= 0 && (y - i) >= 0)
        {

            //prise ou colision

            if (e.tab[x - i, y-i].getValue() != 0)
            {
                if (e.tab[x - i, y-i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y -i) * 8 + x -i], this.x, this.y, x-i, y - i, (e.tab[x -i, y -i])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y -i) * 8 + x - i], this.x, this.y, x-i, y - i, (e.tab[x - i, y - i])));
            }

            i++;

        }
        //déplacement droite/bas
        i = 1;
        while ((x + i) < 8 && (y + i) < 8)
        {

            //prise ou colision
            if (e.tab[x + i, y+i].getValue() != 0)
            {
                if (e.tab[x + i, y+i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x + i], this.x, this.y, x+i, y +i, (e.tab[x + i, y + i])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x + i], this.x, this.y, x+i,y +i, (e.tab[x + i, y + i])));
            }
            i++;

        }
        //déplacement haut/droite
        i = 1;
        while ((y - i) >= 0 && (x + i) < 8)
        {

            //prise ou colision
            if (e.tab[x+i, y - i].getValue() != 0)
            {
                if (e.tab[x+i, y - i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y- i) * 8 + x + i], this.x, this.y, x+i, y - i, (e.tab[x + i, y - i])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y - i) * 8 + x + i], this.x, this.y, x+i, y - i, (e.tab[x + i, y - i])));
            }
            i++;

        }
        //déplacement bas gauche
        i = 1;
        while ((y + i) < 8 && (x - i) >= 0)
        {
            //prise ou colision
            if (e.tab[x-i, y + i].getValue() != 0)
            {
                if (e.tab[x-i, y + i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x - i], this.x, this.y, x-i, y +i, (e.tab[x - i, y + i])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x - i], this.x, this.y, x-i, y +i, (e.tab[x - i, y + i])));
            }
            i++;

        }

    }
    public override void yourPlays()
    {

        //déplacement gauche/haut
        int i = 1;
        while ((x - i) >= 0 && (y - i) >= 0)
        {

            //prise ou colision

            if (e.tab[x - i, y - i].getValue() != 0)
            {
                if (!e.tab[x - i, y - i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y - i) * 8 + x - i], this.x, this.y, x - i, y - i, (e.tab[x - i, y - i])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y - i) * 8 + x - i], this.x, this.y, x - i, y - i, (e.tab[x - i, y - i])));
            }

            i++;

        }
        //déplacement droite/bas
        i = 1;
        while ((x + i) < 8 && (y + i) < 8)
        {

            //prise ou colision
            if (e.tab[x + i, y + i].getValue() != 0)
            {
                if (!e.tab[x + i, y + i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x + i], this.x, this.y, x + i, y + i, (e.tab[x + i, y + i])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x + i], this.x, this.y, x + i, y + i, (e.tab[x + i, y + i])));
            }
            i++;

        }
        //déplacement haut/droite
        i = 1;
        while ((y - i) >= 0 && (x + i) < 8)
        {

            //prise ou colision
            if (e.tab[x + i, y - i].getValue() != 0)
            {
                if (!e.tab[x + i, y - i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y - i) * 8 + x + i], this.x, this.y, x + i, y - i, (e.tab[x + i, y - i])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y - i) * 8 + x + i], this.x, this.y, x + i, y - i, (e.tab[x + i, y - i])));
            }
            i++;

        }
        //déplacement bas gauche
        i = 1;
        while ((y + i) < 8 && (x - i) >= 0)
        {
            //prise ou colision
            if (e.tab[x - i, y + i].getValue() != 0)
            {
                if (!e.tab[x - i, y + i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x - i], this.x, this.y, x - i, y + i, (e.tab[x - i, y + i])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x - i], this.x, this.y, x - i, y + i, (e.tab[x - i, y + i])));
            }
            i++;

        }

    }
}
public class Dame : Piece
{
    public Dame(bool color, int x, int y, Echiquier e) : base(90, color, x, y, e) { }
    public override void myPlays()
    {
        //déplacement gauche
        int i = 1;
        while ((x - i) >= 0)
        {

            //prise ou colision

            if (e.tab[x - i, y].getValue() != 0)
            {
                if (e.tab[x - i, y].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y) * 8 + x - i], this.x, this.y, x - i, y, (e.tab[x - i, y])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y) * 8 + x - i], this.x, this.y, x - i, y, (e.tab[x - i, y])));
            }

            i++;

        }
        //déplacement droite
        i = 1;
        while ((x + i) < 8)
        {

            //prise ou colision
            if (e.tab[x + i, y].getValue() != 0)
            {
                if (e.tab[x + i, y].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y) * 8 + x + i], this.x, this.y, x + i, y, (e.tab[x + i, y])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y) * 8 + x + i], this.x, this.y, x + i, y, (e.tab[x + i, y])));
            }
            i++;

        }
        //déplacement bas
        i = 1;
        while ((y - i) >= 0)
        {

            //prise ou colision
            if (e.tab[x, y - i].getValue() != 0)
            {
                if (e.tab[x, y - i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y - i) * 8 + x], this.x, this.y, x, y - i, (e.tab[x, y - i])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y - i) * 8 + x], this.x, this.y, x, y - i, (e.tab[x, y - i])));

            }
            i++;

        }
        //déplacement haut
        i = 1;
        while ((y + i) < 8)
        {
            //prise ou colision
            if (e.tab[x, y + i].getValue() != 0)
            {
                if (e.tab[x, y + i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x], this.x, this.y, x, y + i, (e.tab[x, y + i])));

                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x], this.x, this.y, x, y + i, (e.tab[x, y + i])));
            }
            i++;

        }

        //déplacement gauche/haut
        i = 1;
        while ((x - i) >= 0 && (y - i) >= 0)
        {

            //prise ou colision

            if (e.tab[x - i, y - i].getValue() != 0)
            {
                if (e.tab[x - i, y - i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y - i) * 8 + x - i], this.x, this.y, x - i, y - i, (e.tab[x - i, y - i])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y - i) * 8 + x - i], this.x, this.y, x - i, y - i, (e.tab[x - i, y - i])));
            }

            i++;

        }
        //déplacement droite/bas
        i = 1;
        while ((x + i) < 8 && (y + i) < 8)
        {

            //prise ou colision
            if (e.tab[x + i, y + i].getValue() != 0)
            {
                if (e.tab[x + i, y + i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x + i], this.x, this.y, x + i, y + i, (e.tab[x + i, y + i])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x + i], this.x, this.y, x + i, y + i, (e.tab[x + i, y + i])));
            }
            i++;

        }
        //déplacement haut/droite
        i = 1;
        while ((y - i) >= 0 && (x + i) < 8)
        {

            //prise ou colision
            if (e.tab[x + i, y - i].getValue() != 0)
            {
                if (e.tab[x + i, y - i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y - i) * 8 + x + i], this.x, this.y, x + i, y - i, (e.tab[x + i, y - i])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y - i) * 8 + x + i], this.x, this.y, x + i, y - i, (e.tab[x + i, y - i])));
            }
            i++;

        }
        //déplacement bas gauche
        i = 1;
        while ((y + i) < 8 && (x - i) >= 0)
        {
            //prise ou colision
            if (e.tab[x - i, y + i].getValue() != 0)
            {
                if (e.tab[x - i, y + i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x - i], this.x, this.y, x - i, y + i, (e.tab[x - i, y + i])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x - i], this.x, this.y, x - i, y + i, (e.tab[x - i, y + i])));
            }
            i++;

        }
    }
    public override void yourPlays()
    {
        //déplacement gauche
        int i = 1;
        while ((x - i) >= 0)
        {

            //prise ou colision

            if (e.tab[x - i, y].getValue() != 0)
            {
                if (!e.tab[x - i, y].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y) * 8 + x - i], this.x, this.y, x - i, y, (e.tab[x - i, y])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y) * 8 + x - i], this.x, this.y, x - i, y, (e.tab[x - i, y])));
            }

            i++;

        }
        //déplacement droite
        i = 1;
        while ((x + i) < 8)
        {

            //prise ou colision
            if (e.tab[x + i, y].getValue() != 0)
            {
                if (!e.tab[x + i, y].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y) * 8 + x + i], this.x, this.y, x + i, y, (e.tab[x + i, y])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y) * 8 + x + i], this.x, this.y, x + i, y, (e.tab[x + i, y])));
            }
            i++;

        }
        //déplacement bas
        i = 1;
        while ((y - i) >= 0)
        {

            //prise ou colision
            if (e.tab[x, y - i].getValue() != 0)
            {
                if (!e.tab[x, y - i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y - i) * 8 + x], this.x, this.y, x, y - i, (e.tab[x, y - i])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y - i) * 8 + x], this.x, this.y, x, y - i, (e.tab[x, y - i])));

            }
            i++;

        }
        //déplacement haut
        i = 1;
        while ((y + i) < 8)
        {
            //prise ou colision
            if (e.tab[x, y + i].getValue() != 0)
            {
                if (!e.tab[x, y + i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x], this.x, this.y, x, y + i, (e.tab[x, y + i])));

                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x], this.x, this.y, x, y + i, (e.tab[x, y + i])));
            }
            i++;

        }

        //déplacement gauche/haut
        i = 1;
        while ((x - i) >= 0 && (y - i) >= 0)
        {

            //prise ou colision

            if (e.tab[x - i, y - i].getValue() != 0)
            {
                if (!e.tab[x - i, y - i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y - i) * 8 + x - i], this.x, this.y, x - i, y - i, (e.tab[x - i, y - i])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y - i) * 8 + x - i], this.x, this.y, x - i, y - i, (e.tab[x - i, y - i])));
            }

            i++;

        }
        //déplacement droite/bas
        i = 1;
        while ((x + i) < 8 && (y + i) < 8)
        {

            //prise ou colision
            if (e.tab[x + i, y + i].getValue() != 0)
            {
                if (!e.tab[x + i, y + i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x + i], this.x, this.y, x + i, y + i, (e.tab[x + i, y + i])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x + i], this.x, this.y, x + i, y + i, (e.tab[x + i, y + i])));
            }
            i++;

        }
        //déplacement haut/droite
        i = 1;
        while ((y - i) >= 0 && (x + i) < 8)
        {

            //prise ou colision
            if (e.tab[x + i, y - i].getValue() != 0)
            {
                if (!e.tab[x + i, y - i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y - i) * 8 + x + i], this.x, this.y, x + i, y - i, (e.tab[x + i, y - i])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y - i) * 8 + x + i], this.x, this.y, x + i, y - i, (e.tab[x + i, y - i])));
            }
            i++;

        }
        //déplacement bas gauche
        i = 1;
        while ((y + i) < 8 && (x - i) >= 0)
        {
            //prise ou colision
            if (e.tab[x - i, y + i].getValue() != 0)
            {
                if (!e.tab[x - i, y + i].getColor())
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x - i], this.x, this.y, x - i, y + i, (e.tab[x - i, y + i])));
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                e.LesCoups.Add(new Coups(this.position, tabCoord[(y + i) * 8 + x - i], this.x, this.y, x - i, y + i, (e.tab[x - i, y + i])));
            }
            i++;

        }
    }
}

public class Roi : Piece
{
    public Roi(bool color, int x, int y, Echiquier e) : base(900, color, x, y, e) { }
    public override void myPlays()
    {
        string s = "";

        int[][] offsets = new int[][] {
        new int[] {-1, -1},
        new int[] {-1, 1},
        new int[] {-1, 0},
        new int[] {0, 1},
        new int[] {0, -1},
        new int[] {1, 1},
        new int[] {1, -1},
        new int[] {1, 0}
    };
        foreach (int[] o in offsets)
        {

            int moveX = o[0];
            int moveY = o[1];
            if (0 <= x + moveX && x + moveX <= 7 && y + moveY >= 0 && y + moveY <= 7)
                //Si prise ou colision
                if (e.tab[x + moveX, y + moveY].getValue() != 0)
                {
                    if (e.tab[x + moveX, y + moveY].getColor())
                    {
                        e.LesCoups.Add(new Coups(this.position, tabCoord[(y + moveY) * 8 + x + moveX], this.x, this.y, x+moveX, y + moveY, (e.tab[x + moveX, y + moveY])));

                    }
                }
                else
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y + moveY) * 8 + x + moveX], this.x, this.y, x + moveX, y + moveY, (e.tab[x + moveX, y + moveY])));


                }
        }
        
    }
    public override void yourPlays()
    {
        string s = "";

        int[][] offsets = new int[][] {
        new int[] {-1, -1},
        new int[] {-1, 1},
        new int[] {-1, 0},
        new int[] {0, 1},
        new int[] {0, -1},
        new int[] {1, 1},
        new int[] {1, -1},
        new int[] {1, 0}
    };
        foreach (int[] o in offsets)
        {

            int moveX = o[0];
            int moveY = o[1];
            if (0 <= x + moveX && x + moveX <= 7 && y + moveY >= 0 && y + moveY <= 7)
                //Si prise ou colision
                if (e.tab[x + moveX, y + moveY].getValue() != 0)
                {
                    if (!e.tab[x + moveX, y + moveY].getColor())
                    {
                        e.LesCoups.Add(new Coups(this.position, tabCoord[(y + moveY) * 8 + x + moveX], this.x, this.y, x + moveX, y + moveY, (e.tab[x + moveX, y + moveY])));

                    }
                }
                else
                {
                    e.LesCoups.Add(new Coups(this.position, tabCoord[(y + moveY) * 8 + x + moveX], this.x, this.y, x + moveX, y + moveY, (e.tab[x + moveX, y + moveY])));


                }
        }

    }
}
