using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Coups
{
    public String positionDepart;
    public String positionArrivee;
    public int xDepart;
    public int yDepart;
    public int xArrivee;
    public int yArrivee;
    public Piece prise;
    public int value = 0;

    int Value;
    public Coups(String positionDepart, String positionArrivee, int xDepart, int yDepart, int xArrivee, int yArrivee, Piece prise)
    {
        this.Value = prise.getValue();
        this.positionArrivee = positionArrivee;
        this.positionDepart = positionDepart;
        this.xDepart = xDepart;
        this.yDepart = xDepart;
        this.xArrivee = xArrivee;
        this.yArrivee = yArrivee;
        this.prise = prise;
        this.value = prise.getValue();


    }
    public int getValue()
    {
        return this.Value;
    }


}
